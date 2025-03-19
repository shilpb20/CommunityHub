using AutoMapper;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Services.Registration;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.Api.Controllers
{
    [ApiController]
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

        [HttpGet(ApiRouteSegment.AdminRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RegistrationRequestDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<RegistrationRequestDto>>> GetRequests([FromQuery] string status = "pending")
        {
            if (Enum.TryParse<RegistrationStatus>(status, true, out var registrationStatus)
                && Enum.IsDefined(typeof(RegistrationStatus), registrationStatus))
            {
                var requests = await _registrationService.GetRequestsAsync(registrationStatus);
                if (!requests.Any()) { return NoContent(); }

                return Ok(_mapper.Map<List<RegistrationRequestDto>>(requests));
            }
            else
            {
                return BadRequest($"Invalid registration status value: {status}. Valid values are {string.Join(", ", Enum.GetNames(typeof(RegistrationStatus)))}.");
            }
        }

        [HttpPut(ApiRouteSegment.RejectRequest + "/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationRequestDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegistrationRequestDto>> RejectRequest(int id, [FromBody] string comment)
        {
            try
            {
                if (id <= 0) return BadRequest("Id must be a positive number.");

                var registrationRequest = await _registrationService.RejectRequestAsync(id, comment);
                if (registrationRequest == null) return BadRequest($"Request with ID {id} not found or could not be rejected.");

                return Ok(_mapper.Map<RegistrationRequestDto>(registrationRequest));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while rejecting request - {id}", id);
                return StatusCode(500, $"Unexpected error occurred while rejecting request - {id}");
            }
        }

        [HttpPost(ApiRouteSegment.ApproveRequest+ "/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInfoDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserInfoDto>> ApproveRequest(int id)
        {
            if (id <= 0) return BadRequest("Id must be a positive number.");

            var userInfo = await _registrationService.ApproveRequestAsync(id);
            if (userInfo == null) return NotFound($"Request with ID {id} not found or could not be approved.");

            return Ok(_mapper.Map<UserInfoDto>(userInfo));
        }

    }
}
