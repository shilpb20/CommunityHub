using AutoMapper;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CommunityHub.IntegrationTests.Controllers
{
    public abstract class BaseTestEnv : IClassFixture<ApplicationStartup>
    {
        protected readonly string _url;
        protected readonly ApplicationStartup _application;
        protected readonly HttpClient _httpClient;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ApplicationDbContext _dbContext;

        protected readonly IMapper _mapper;
        protected readonly IRegistrationService _registrationService;

        public BaseTestEnv(ApplicationStartup application, string url)
        {
            _url = url;
            _application = application;
            _httpClient = _application.Client;

            _serviceProvider = _application.WebApplicationFactory.Services;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
            _dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();

            using (var scope = _serviceProvider.CreateScope())
            {
                _registrationService = scope.ServiceProvider.GetRequiredService<IRegistrationService>();   
            }
        }
    }
}
