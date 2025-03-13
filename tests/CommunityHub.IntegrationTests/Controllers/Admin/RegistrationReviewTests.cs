using AutoMapper;
using Azure.Core;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;
using CommunityHub.Core.Helpers;
using CommunityHub.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public RegistrationReviewTests(ApplicationStartup application) : base(application)
        {
            _url = "api/admin/request";
        }

        async Task<List<RegistrationRequestDto>> SeedRegistrationRequests()
        {
            var createdRequests = new List<RegistrationRequestDto>();

            var registrationRequests = RegistrationDataList.GetRequests();
            foreach (var registrationData in registrationRequests)
            {
                var result = await HttpSendRequestHelper
                    .SendPostRequestAsync<RegistrationDataCreateDto, RegistrationRequestDto>(_httpClient, "api/account", registrationData);

                Assert.Equivalent(registrationData, result.RegistrationData);

                createdRequests.Add(result);
            }

            return createdRequests;
        }


        [Theory]
        [InlineData("")]
        [InlineData("all")]
        [InlineData("pending")]
        public async Task GetRequests_ShouldReturnSuccessAndAllPendingRequests_WhenValidRequestIsSent(string registrationStatus)
        {
            //Arrange
            await SeedRegistrationRequests();

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


        [Theory]
        [InlineData("approved")]
        [InlineData("rejected")]
        public async Task GetRequests_ShouldReturnNoContent_WhenMatchingDataIsNotFound(string registrationStatus)
        {
            //Arrange
            await SeedRegistrationRequests();

            var uriBuilder = new UriBuilder(_application.Client.BaseAddress)
            {
                Path = _url.TrimStart('/'),
                Query = $"registrationStatus={registrationStatus}"
            };


            //Act
            var request = HttpHelper.GetHttpGetRequest(uriBuilder.ToString());
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status204NoContent, response.StatusCode);
        }

        [Fact]
        public async Task GetRequests_ShouldReturnBadRequest_WhenInvalidStatusIsSent()
        {
            //Arrange
            await SeedRegistrationRequests();

            var registrationStatus = "invalid";
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


        [Fact]
        public async Task RejectRequest_ShouldReturnSuccessAndUpdateRegistrationRequest_WhenValidRequestIsSent()
        {
            //Arrange
            int id = 1;
            string rejectUrl = _url + "/reject";
            var reviewComment = "Duplicate request";
            var requestsCreated = await SeedRegistrationRequests();
            var expectedResult = requestsCreated.FirstOrDefault();

            var uriBuilder = new UriBuilder(_application.Client.BaseAddress)
            {
                Path = rejectUrl.TrimStart('/'),
                Query = $"id={id}&reviewComment={Uri.EscapeDataString(reviewComment)}"
            };

            //Act
            var result = await HttpSendRequestHelper.SendUpdateRequestAsync<RegistrationRequestDto>(_httpClient, uriBuilder.ToString());

            //Assert
            Assert.Equivalent(RegistrationStatus.Rejected.GetEnumMemberValue(), result.RegistrationStatus.ToLower());
            Assert.Equivalent(reviewComment, result.Review);

            Assert.NotNull(result.ReviewedAt);
            Assert.InRange(result.ReviewedAt.Value, DateTime.UtcNow.AddMilliseconds(-500), DateTime.UtcNow);

            Assert.Equivalent(expectedResult.RegistrationData, result.RegistrationData);
            Assert.Equivalent(expectedResult.CreatedAt, result.CreatedAt);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task RejectRequest_ShouldReturnBadRequest_WhenNonPositiveIdIsSent(int id)
        {
            //Arrange
            var requestsCreated = await SeedRegistrationRequests();

            string rejectUrl = _url + "/reject";
            var reviewComment = "Duplicate request";
            var uriBuilder = new UriBuilder(_application.Client.BaseAddress)
            {
                Path = rejectUrl.TrimStart('/'),
                Query = $"id={id}&reviewComment={Uri.EscapeDataString(reviewComment)}"
            };

            //Act
            HttpRequestMessage request = HttpHelper.GetHttpPutRequest(uriBuilder.ToString());
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RejectRequest_ShouldReturnBadRequest_WhenInvalidIdIsSent()
        {
            //Arrange
            var requestsCreated = await SeedRegistrationRequests();
            int id = _dbContext.RegistrationRequests.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;

            string rejectUrl = _url + "/reject";
            var reviewComment = "Invalid data";
            var uriBuilder = new UriBuilder(_application.Client.BaseAddress)
            {
                Path = rejectUrl.TrimStart('/'),
                Query = $"id={id}&reviewComment={Uri.EscapeDataString(reviewComment)}"
            };

            //Act
            HttpRequestMessage request = HttpHelper.GetHttpPutRequest(uriBuilder.ToString());
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status400BadRequest, response.StatusCode);
        }
    }
}
