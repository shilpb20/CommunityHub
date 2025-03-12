using AppComponents.Repository.Abstraction;
using AutoMapper;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos.RegistrationData;
using Newtonsoft.Json;

namespace CommunityHub.Api.Controllers
{
    [ApiController]
    [Route(ApiRoute.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IMapper _mapper;
        private readonly IRegistrationService _registrationService;

        public AdminController(
            ILogger<AdminController> logger, 
            IMapper mapper, 
            IRegistrationService registrationService)
        {
            _logger = logger;
            _mapper = mapper;
            _registrationService = registrationService;
        }

        [HttpGet(Name ="GetRequests")]
        public async Task<ActionResult<List<RegistrationRequestDto>>> GetRequests([FromQuery]string registrationStatus = "pending")
        {
            if (Enum.TryParse<RegistrationStatus>(registrationStatus, true, out var status) 
                && Enum.IsDefined(typeof(RegistrationStatus), status))
            {
                var requests = await _registrationService.GetRequestsAsync(status);
                var requestDtos = _mapper.Map < List<RegistrationRequestDto>>(requests);
                return Ok(requestDtos);
            }
            else
            {
                return BadRequest($"Invalid registration status value: {registrationStatus}. Valid values are {string.Join(", ", Enum.GetNames(typeof(RegistrationStatus)))}.");
            }
        }
    }
}
