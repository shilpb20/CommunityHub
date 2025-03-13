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
using Microsoft.AspNetCore.Http.HttpResults;

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

        [HttpGet("request")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RegistrationRequestDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<RegistrationRequestDto>>> GetRequests([FromQuery]string registrationStatus = "pending")
        {
            if (Enum.TryParse<RegistrationStatus>(registrationStatus, true, out var status) 
                && Enum.IsDefined(typeof(RegistrationStatus), status))
            {
                var requests = await _registrationService.GetRequestsAsync(status);

                if(!requests.Any())
                {
                    return NoContent();
                }

                var requestDtos = _mapper.Map < List<RegistrationRequestDto>>(requests);
                return Ok(requestDtos);
            }
            else
            {
                return BadRequest($"Invalid registration status value: {registrationStatus}. Valid values are {string.Join(", ", Enum.GetNames(typeof(RegistrationStatus)))}.");
            }
        }

        [HttpPut("request/reject")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationRequestDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegistrationRequestDto>> RejectRequest(int id, string reviewComment)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Id must be a positive value");
                }

                var matchingRequest = await _registrationService.GetRequestAsync(id);
                if (matchingRequest == null)
                {
                    return BadRequest("Matching data not found");
                }

                var registrationRequest = await _registrationService.RejectRequestAsync(id, reviewComment);
                var registrationRequestDto = _mapper.Map<RegistrationRequestDto>(registrationRequest);
                return Ok(registrationRequestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while rejecting request - {id}", id);
                return StatusCode(500, $"Unexpected error occurred while rejecting request - {id}");
            }

        }
    }
}
