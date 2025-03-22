using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;
using CommunityHub.Core.Helpers;
using CommunityHub.Infrastructure.Models;
using CommunityHub.IntegrationTests;
using CommunityHub.IntegrationTests.TestData;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CommunityHub.Core.Constants;

namespace CommunityHub.Integrations.Controllers.Admin
{
    public class RegistrationReviewTests : BaseTestEnv, IAsyncLifetime
    {
        private const string _registerRoute = "api/account";
        private List<RegistrationRequestDto> _registrationRequests;

        public RegistrationReviewTests(ApplicationStartup application) : base(application)
        {
            _url = ApiRouteSegment.AdminRequest;
        }

        public async Task InitializeAsync()
        {
            _registrationRequests = await SeedRegistrationRequests();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }


        public async Task<List<RegistrationRequestDto>> SeedRegistrationRequests()
        {
            var createdRequests = new List<RegistrationRequestDto>();

            var registrationRequests = RegistrationDataList.GetRequests();
            foreach (var registrationData in registrationRequests)
            {
                var result = await HttpSendRequestHelper
                    .SendPostRequestAsync<RegistrationInfoCreateDto, RegistrationRequestDto>(_httpClient, "api/account", registrationData);

                Assert.Equivalent(registrationData, result.RegistrationInfo);

                createdRequests.Add(result);
            }

            return createdRequests;
        }

        #region get- requests

        [Theory]
        [InlineData("")]
        [InlineData("all")]
        [InlineData("pending")]
        public async Task GetRequests_ShouldReturnSuccessAndAllPendingRequests_WhenValidRequestIsSent(string registrationStatus)
        {
            //Arrange
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
            Dictionary<string, string> queryParameters = new Dictionary<string, string>()
            {
               { RouteParameter.Request.RegistrationStatus, registrationStatus }
            };
            var uriBuilder = HttpHelper.BuildUri(_httpClient.BaseAddress.ToString(), ApiRouteSegment.AdminRequest, queryParameters);

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
            var registrationStatus = "invalid";
            Dictionary<string, string> queryParameters = new Dictionary<string, string>()
            {
               { RouteParameter.Request.RegistrationStatus, registrationStatus }
            };
            var uriBuilder = HttpHelper.BuildUri(_httpClient.BaseAddress.ToString(), ApiRouteSegment.AdminRequest, queryParameters);


            //Act
            var request = HttpHelper.GetHttpGetRequest(uriBuilder.ToString());
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        #endregion


        #region reject-requests

        [Fact]
        public async Task RejectRequest_ShouldReturnSuccessAndUpdateRegistrationRequest_WhenValidRequestIsSent()
        {
            //Arrange
            string comment = "Duplicate request";
            var expectedResult = _registrationRequests.FirstOrDefault();

            //Act
            var result = await HttpSendRequestHelper
                .SendUpdateRequestAsync<string, RegistrationRequestDto>
                (_httpClient, ApiRouteSegment.RejectRequest, expectedResult.Id, comment);

            //Assert
            Assert.Equivalent(RegistrationStatus.Rejected.GetEnumMemberValue().ToLower(), result.RegistrationStatus.ToLower());
            Assert.Equivalent(comment, result.Review);

            Assert.NotNull(result.ReviewedAt);
            Assert.InRange(result.ReviewedAt.Value, DateTime.UtcNow.AddMilliseconds(-500), DateTime.UtcNow);

            Assert.Equivalent(expectedResult.RegistrationInfo, result.RegistrationInfo);
            Assert.Equivalent(expectedResult.CreatedAt, result.CreatedAt);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task RejectRequest_ShouldReturnBadRequest_WhenNonPositiveIdIsSent(int id)
        {
            //Arrange
            string comment = "Duplicate request";
            var expectedResult = _registrationRequests.FirstOrDefault();

            //Act
            HttpRequestMessage request = HttpHelper.GetHttpPutRequest<string>(ApiRouteSegment.RejectRequest, id, comment);
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RejectRequest_ShouldReturnBadRequest_WhenInvalidIdIsSent()
        {
            //Arrange
            int id = _dbContext.RegistrationRequests.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            string comment = "Invalid request";

            //Act
            HttpRequestMessage request = HttpHelper.GetHttpPutRequest<string>(ApiRouteSegment.RejectRequest, id, comment);
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        #endregion

        #region approve requests

        [Fact]
        public async Task ApproveRequest_ShouldReturnCreatedAndRegisterUser()
        {
            //Arrange
            RegistrationRequest registrationRequest = _dbContext.RegistrationRequests.FirstOrDefault(
                    x => x.RegistrationStatus == RegistrationStatus.Pending.GetEnumMemberValue());

            string approveRoute = $"{ApiRouteSegment.ApproveRequest}/{registrationRequest.Id}";
            Assert.Equivalent(_dbContext.UserInfo.Count(), 0);

            //Act
            var request = HttpHelper.GetHttpPostRequest<string>(approveRoute, null);
            var response = await _httpClient.SendAsync(request);
            var result = await HttpHelper.GetHttpResponseObject<UserInfoDto>(response);

            //Assert
            Assert.Equivalent(StatusCodes.Status200OK, response.StatusCode);

            var matchingRegistration = JsonConvert.DeserializeObject<RegistrationInfo>(registrationRequest.RegistrationInfo);
            Assert.Equivalent(matchingRegistration.UserInfo.Email, result.Email);

            //TODO: Match objects
        }

        #endregion
    }
}
