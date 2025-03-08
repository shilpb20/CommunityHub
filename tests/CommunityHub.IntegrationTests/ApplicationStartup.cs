using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace CommunityHub.IntegrationTests
{
    public class ApplicationStartup : IClassFixture<WebApplicationFactory<Program>>
    {
        public readonly WebApplicationFactory<Program> WebApplicationFactory;
        public readonly HttpClient Client = new HttpClient();

        public ApplicationStartup()
        {
            WebApplicationFactory = new WebApplicationFactory<Program>();
            Client = WebApplicationFactory.CreateClient();
        }
    }
}
