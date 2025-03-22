using CommunityHub.Core.Dtos;
using CommunityHub.Core.Helpers;
using CommunityHub.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
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
        public async Task RegisterUser_ReturnsCreatedAtResult_WhenValidDataIsSent()
        {
            RegistrationInfoCreateDto registrationDataCreateDto = GetRegistrationDataCreateDto();

            var registrationData = _mapper.Map<RegistrationInfoDto>(registrationDataCreateDto);
            var registrationRequest = new RegistrationRequest()
            {
                RegistrationInfo = JsonConvert.SerializeObject(registrationData),
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

            Assert.Equivalent(registrationData, result.RegistrationInfo);
            Assert.Equivalent("pending", result.RegistrationStatus);
            Assert.InRange(result.CreatedAt, DateTime.UtcNow.AddMilliseconds(-500), DateTime.UtcNow);
            Assert.Null(result.ReviewedAt);
            Assert.Null(result.Review);
        }

        [Fact]
        public async Task RegisterUser_ReturnsBadRequest_WhenNullDataIsSent()
        {
            //Act
            var request = HttpHelper.GetHttpPostRequest<RegistrationRequestDto>(_url, null);
            request.Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        private static RegistrationInfoCreateDto GetRegistrationDataCreateDto()
        {
            //Arrange
            return new RegistrationInfoCreateDto()
            {
                UserInfo = new UserInfoCreateDto()
                {
                    FullName = "Alice Smith",
                    Email = "Alice.smith@gmail.com",
                    CountryCode = "+61",
                    ContactNumber = "0410203040",
                    Location = "Sydney",
                    Gender = "Female",
                    MaritalStatus = "Married",
                    HomeTown = "Beranki",
                    HouseName = "Kaadu mane"
                },
                SpouseInfo = new SpouseInfoCreateDto()
                {
                    FullName = "Janet Smith",
                    Email = "janet.smith@gmail.com",
                    CountryCode = "+61",
                    ContactNumber = "0420202020",
                    HomeTown = "Sydney",
                    HouseName = "Smith Family"
                },
                Children = new List<ChildrenCreateDto>()
                {
                    new() { Name = "Anna" },
                    new() { Name = "Benny" },
                    new() { Name = "Cairo" }
                }
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

        [Fact]
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
            var request = HttpHelper.GetHttpGetRequestById(_url, id);
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
            RegistrationInfoCreateDto registrationData = GetRegistrationDataCreateDto();
            return await HttpSendRequestHelper.SendPostRequestAsync<RegistrationInfoCreateDto, RegistrationRequestDto>(_httpClient, _url, registrationData);
        }
    }
}
