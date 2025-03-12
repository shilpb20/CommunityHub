using AutoMapper;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Helpers;
using CommunityHub.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.IntegrationTests.Controllers.Admin
{
    public class RegistrationReviewTests : BaseTestEnv
    {
        private const string _registerRoute = "api/account";
        public RegistrationReviewTests(ApplicationStartup application, string url = "api/admin/requests") : base(application, url)
        {
        }



        [Theory]
        [InlineData("all")]
        [InlineData("")]
        [InlineData("pending")]
        [InlineData("approved")]
        [InlineData("rejected")]
        public async Task GetRequests_ShouldReturnSuccessAndAllPendingRequests_WhenValidRequestIsSent(string registrationStatus)
        {
            //Arrange
            var registrationRequests = RegistrationDataList.GetRequests();
            foreach(var registrationData in registrationRequests)
            {
                var response = await HttpSendRequestHelper
                    .SendPostRequest<RegistrationDataCreateDto, RegistrationRequestDto>(_httpClient, _registerRoute, registrationData);
                Assert.Equivalent(registrationData, response.RegistrationData);
            }

            var uriBuilder = new UriBuilder(_application.Client.BaseAddress)
            {
                Path = _url.TrimStart('/'),
                Query = $"registrationStatus={registrationStatus}"
            };


            //Act
            var result = await HttpSendRequestHelper.SendGetRequestAsync<List<RegistrationRequestDto>>(_httpClient, uriBuilder.ToString());

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetRequests_ShouldReturnBadRequest_WhenInvalidStatusIsSent()
        {
            //Arrange
            var registrationStatus = "invalid";
            var registrationRequests = RegistrationDataList.GetRequests();
            foreach (var registrationData in registrationRequests)
            {
                var result = await HttpSendRequestHelper
                    .SendPostRequest<RegistrationDataCreateDto, RegistrationRequestDto>(_httpClient, _registerRoute, registrationData);
                Assert.Equivalent(registrationData, result.RegistrationData);
            }

            var uriBuilder = new UriBuilder(_application.Client.BaseAddress)
            {
                Path = _url.TrimStart('/'),
                Query = $"registrationStatus={registrationStatus}"
            };


            //Act
            var request = HttpHelper.GetHttpGetRequest(uriBuilder.ToString());
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status400BadRequest, response.StatusCode);
        }


    }
}
