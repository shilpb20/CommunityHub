using CommunityHub.Tests.Core.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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

        public AuthController_RegistrationTests(ApplicationStartup application)
        {
            _application = application;
            _httpClient = _application.Client;
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnBadRequest_WhenInvalidDataIsSent()
        {
            //Arrange

            //Act
            var request = HttpHelper.GetHttpPostRequest<object>(_url, null);
            var response = await _httpClient.SendAsync(request);

            //Assert
            Assert.Equivalent(response.StatusCode, StatusCodes.Status400BadRequest);
        }
    }
}
