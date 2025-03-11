using AutoMapper;
using CommunityHub.Infrastructure.Services;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Dtos.RegistrationData;
using CommunityHub.Core.Helpers;
using CommunityHub.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.Api.Controllers
{
    [ApiController]
    [Route("api/register")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly IRegistrationService _registrationService;

        public AuthController(IRegistrationService registrationService, ILogger<AuthController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _registrationService = registrationService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegistrationRequestDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegistrationRequestDto>> AddRegistrationRequest([FromBody] RegistrationDataCreateDto registrationDataDto)
        {
            if (registrationDataDto == null)
            {
                return BadRequest("Registration data must not be null");
            }

            try
            {
                var registrationData = _mapper.Map<RegistrationData>(registrationDataDto);
                var registrationRequest = await _registrationService.CreateRequestAsync(registrationData);
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

        [HttpGet("{id:int}", Name = "GetRegistrationRequest")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationRequestDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegistrationRequestDto>> GetRegistrationRequest(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be a positive number");
            }

            var result = await _registrationService.GetRequestAsync(id);
            if (result == null)
            {
                return NotFound($"Request with Id - {id} is not found.");
            }

            var resultDto = _mapper.Map<RegistrationRequestDto>(result);
            return Ok(resultDto);
        }
    }
}
