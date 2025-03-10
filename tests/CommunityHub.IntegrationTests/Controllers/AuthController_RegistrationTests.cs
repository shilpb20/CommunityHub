using AutoMapper;
using CommunityHub.Infrastructure.Services;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Dtos.RegistrationData;
using CommunityHub.Core.Models;
using CommunityHub.Tests.Core.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.IntegrationTests.Controllers
{
    public class AuthController_RegistrationTests : IClassFixture<ApplicationStartup>
    {
        private readonly string _url = "api/register";

        private readonly ApplicationStartup _application;
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;


        private readonly IMapper _mapper;
        private readonly IRegistrationService _registrationService;

        public AuthController_RegistrationTests(ApplicationStartup application)
        {
            _application = application;
            _httpClient = _application.Client;

            _serviceProvider = _application.WebApplicationFactory.Services;

            _mapper = _application.WebApplicationFactory.Services.GetRequiredService<IMapper>();

            using (var scope = _serviceProvider.CreateScope())
            {
                _registrationService = scope.ServiceProvider.GetRequiredService<IRegistrationService>();
            }
        }

        #region add-registration-request

        [Fact]
        public async Task RegisterUser_ReturnsBadRequest_WhenNullDataIsSent()
        {
            //Act
            var request = HttpHelper.GetHttpPostRequest<object>(_url, null);
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RegisterUser_ReturnsCreateAt_WhenValidDataIsSent()
        {
            RegistrationDataCreateDto registrationDataCreateDto = GetRegistrationDataCreateDto();

            var registrationData = _mapper.Map<RegistrationData>(registrationDataCreateDto);
            var registrationRequest = new RegistrationRequest()
            {
                RegistrationData = JsonConvert.SerializeObject(registrationData),
                CreatedAt = DateTime.UtcNow,
                RegistrationStatus = "Pending",
                ReviewedAt = null,
                Review = null
            };


            //Act
            var request = HttpHelper.GetHttpPostRequest<RegistrationDataCreateDto>(_url, registrationDataCreateDto);
            var response = await _httpClient.SendAsync(request);
            var result = await HttpHelper.GetHttpResponseObject<RegistrationRequestDto>(response);

            //Assert
            Assert.Equivalent(StatusCodes.Status201Created, response.StatusCode);
            Assert.NotNull(result);

            Assert.Equivalent(registrationData, result.RegistrationData);
            Assert.Equivalent("Pending", result.RegistrationStatus);
            Assert.InRange(DateTime.UtcNow, DateTime.UtcNow.AddMilliseconds(-100), DateTime.UtcNow);
            Assert.Null(result.ReviewedAt);
            Assert.Null(result.Review);
        }

        private static RegistrationDataCreateDto GetRegistrationDataCreateDto()
        {
            //Arrange
            return new RegistrationDataCreateDto()
            {
                UserDetails = new UserDetailsCreateDto()
                {
                    FullName = "Alice Smith",
                    Email = "alice.smith@gmail.com",
                    CountryCode = "+61",
                    ContactNumber = "0410203040",
                    Location = "Sydney",
                    Gender = "Female",
                    MaritalStatus = "Married",
                    HomeTown = "Beranki",
                    HouseName = "Kaadu mane"
                },
                SpouseDetails = new SpouseDetailsCreateDto()
                {
                    FullName = "Janet Smith",
                    Email = "janet.smith@gmail.com",
                    CountryCode = "+61",
                    ContactNumber = "0420202020",
                    HomeTown = "Sydney",
                    HouseName = "Smith Family"
                },
                Children = new List<string>() { "Anna", "Benny", "Cairo" }
            };
        }

        #endregion

        #region get-registration-request

        [Fact]
        public async Task GetRegistrationRequest_ReturnsNotFound_WhenDataIsEmpty()
        {
            //Act
            var request = HttpHelper.GetHttpGetRequestById(_url, 1);
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetRegistrationRequest_ReturnsBadRequest_WhenNonPositiveIdIsSent(int id)
        {
            //Act
            var request = HttpHelper.GetHttpGetRequestById(_url, -1);
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetRegistrationRequest_ReturnsMatchingResult_WhenValidIdIsSent()
        {
            //Arrange
            var registrationRequest = await AddRegistrationRequest();

            //Act
            var request = HttpHelper.GetHttpGetRequestById(_url, registrationRequest.Id);
            var response = await _httpClient.SendAsync(request);
            var result = await HttpHelper.GetHttpResponseObject<RegistrationRequestDto>(response);

            //Assert
            Assert.Equivalent(StatusCodes.Status200OK, response.StatusCode);
            Assert.Equivalent(registrationRequest, result);
        }

        #endregion

        private async Task<RegistrationRequestDto> AddRegistrationRequest()
        {
            RegistrationDataCreateDto registrationData = GetRegistrationDataCreateDto();
            return await HttpHelper.SendHttpPostRequest<RegistrationDataCreateDto, RegistrationRequestDto>(_httpClient, _url, registrationData);
        }
    }
}
