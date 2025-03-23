using AutoMapper;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Services;
using CommunityHub.Infrastructure.Services.Registration;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.Api.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAdminService _adminService;
        private readonly IRegistrationService _registrationService;

        public AccountController(
            ILogger<AccountController> logger,
            IMapper mapper,
            IRegistrationService registrationService,
            IAdminService adminService)
        {
            _logger = logger;
            _mapper = mapper;

            _registrationService = registrationService;
            _adminService = adminService;
        }

        [HttpPost(ApiRoute.Registration.Request)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegistrationRequestDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<RegistrationRequestDto>> AddRegistrationRequest([FromBody] RegistrationInfoCreateDto registrationInfoDto)
        {
            if (registrationInfoDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid registration data");
            }

            try
            {
                var registrationInfo = _mapper.Map<RegistrationInfo>(registrationInfoDto);

                var registrationRequest = await _registrationService.CreateRequestAsync(registrationInfo);
                var registrationRequestDto = _mapper.Map<RegistrationRequestDto>(registrationRequest);

                return CreatedAtRoute("GetRegistrationRequest", new { id = registrationRequestDto.Id }, registrationRequestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing Add Registration Request");
                _logger.LogError($"Add Registration Request error - {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [HttpGet(ApiRoute.Registration.GetRequestById, Name = "GetRegistrationRequest")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationRequestDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegistrationRequestDto>> GetRegistrationRequest(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be a positive number");
            }

            var result = await _registrationService.GetRequestByIdAsync(id);
            if (result == null)
            {
                return NotFound($"Request with Id - {id} is not found.");
            }

            var resultDto = _mapper.Map<RegistrationRequestDto>(result);
            return Ok(resultDto);
        }


        [HttpGet(ApiRoute.Registration.Request)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RegistrationRequestDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<RegistrationRequestDto>>> GetRequests([FromQuery] string status = "pending")
        {
            if (Enum.TryParse<eRegistrationStatus>(status, true, out var registrationStatus)
                && Enum.IsDefined(typeof(eRegistrationStatus), registrationStatus))
            {
                var requests = await _registrationService.GetRequestsAsync(registrationStatus);
                if (!requests.Any()) { return NoContent(); }

                return Ok(_mapper.Map<List<RegistrationRequestDto>>(requests));
            }
            else
            {
                return BadRequest($"Invalid registration status value: {status}. Valid values are {string.Join(", ", Enum.GetNames(typeof(eRegistrationStatus)))}.");
            }
        }
    }
}
