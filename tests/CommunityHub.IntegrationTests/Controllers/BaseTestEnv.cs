using AutoMapper;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Services;
using CommunityHub.Infrastructure.Services.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityHub.IntegrationTests
{
    public abstract class BaseTestEnv : IClassFixture<ApplicationStartup>
    {
        protected readonly ApplicationStartup _application;
        protected readonly HttpClient _httpClient;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ApplicationDbContext _dbContext;

        protected readonly IMapper _mapper;

        protected readonly IRegistrationService _registrationService;

        public BaseTestEnv(ApplicationStartup application)
        {
            _application = application;
            _httpClient = _application.Client;

            _serviceProvider = _application.WebApplicationFactory.Services;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
            _dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();

            using (var scope = _serviceProvider.CreateScope())
            {
                _registrationService = scope.ServiceProvider.GetRequiredService<IRegistrationService>();

                //_adminService = scope.ServiceProvider.GetRequiredService<IAdminService>();
            }
        }
    }
}
