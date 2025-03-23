using AutoMapper;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.DataManagers;
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

        protected readonly IRegistrationRequestManager _requestManager;

        protected readonly IAdminService _adminService;

        public BaseTestEnv(ApplicationStartup application)
        {
            _application = application;
            _httpClient = _application.Client;

            _serviceProvider = _application.WebApplicationFactory.Services;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
            _dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();

            using (var scope = _serviceProvider.CreateScope())
            {
                _requestManager = scope.ServiceProvider.GetRequiredService<IRegistrationRequestManager>();

                _adminService = scope.ServiceProvider.GetRequiredService<IAdminService>();
            }
        }
    }
}
