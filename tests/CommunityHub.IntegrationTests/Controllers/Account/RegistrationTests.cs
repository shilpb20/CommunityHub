using AutoMapper;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Helpers;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CommunityHub.IntegrationTests.Controllers.Account
{
    public class RegistrationTests : BaseTestEnv
    {
        public RegistrationTests(ApplicationStartup application) : base(application)
        {
            _url = "api/account";
        }

        #region add-registration-request

        [Fact]
        public async Task RegisterUser_ReturnsBadRequest_WhenNullDataIsSent()
        {
            //Act
            var request = HttpHelper.GetHttpPostRequest<RegistrationRequestDto>(_url, null);
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RegisterUser_ReturnsCreatedAtResult_WhenValidDataIsSent()
        {
            RegistrationDataCreateDto registrationDataCreateDto = GetRegistrationDataCreateDto();

            var registrationData = _mapper.Map<RegistrationData>(registrationDataCreateDto);
            var registrationRequest = new RegistrationRequest()
            {
                RegistrationData = JsonConvert.SerializeObject( registrationData),
                CreatedAt = DateTime.UtcNow,
                RegistrationStatus = "Pending",
                ReviewedAt = null,
                Review = null
            };


            //Act
            var request = HttpHelper.GetHttpPostRequest(_url, registrationDataCreateDto);
            var response = await _httpClient.SendAsync(request);
            var result = await HttpHelper.GetHttpResponseObject<RegistrationRequestDto>(response);

            //Assert
            Assert.Equivalent(StatusCodes.Status201Created, response.StatusCode);
            Assert.NotNull(result);

            Assert.Equivalent(registrationData, result.RegistrationData);
            Assert.Equivalent("pending", result.RegistrationStatus);
            Assert.InRange(result.CreatedAt, DateTime.UtcNow.AddMilliseconds(-500), DateTime.UtcNow);
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
            //Arrange
            await ClearRegistrationRequestsAsync();

            //Act
            var request = HttpHelper.GetHttpGetRequestById(_url, 1);
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status404NotFound, response.StatusCode);
        }

        public async Task ClearRegistrationRequestsAsync()
        {
            var allRequests = _dbContext.RegistrationRequests.ToList();
            _dbContext.RegistrationRequests.RemoveRange(allRequests);
            await _dbContext.SaveChangesAsync();
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
            return await HttpSendRequestHelper.SendPostRequestAsync<RegistrationDataCreateDto, RegistrationRequestDto>(_httpClient, _url, registrationData);
        }
    }
}
