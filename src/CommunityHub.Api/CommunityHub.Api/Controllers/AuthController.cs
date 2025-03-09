using AutoMapper;
using CommunityHub.Infrastructure.Services;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Dtos.RegistrationData;
using CommunityHub.Core.Helpers;
using CommunityHub.Core.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.Api.Controllers
{
    [ApiController]
    [Route("api/")]
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

        //TODO: Status code update
        //TODO: Admin mail trigger after creation
        [Route("register")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegistrationRequestDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegistrationRequestDto>> AddRegistrationRequest([FromBody] RegistrationDataCreateDto? registrationDataDto)
        {
            try
            {
                if (registrationDataDto == null)
                {
                    return BadRequest("Registration data must not be null");
                }

                var registrationData = _mapper.Map<RegistrationData>(registrationDataDto);
                var registrationRequest = await _registrationService.CreateRequestAsync(registrationData);
                var registrationRequestDto = _mapper.Map<RegistrationRequestDto>(registrationRequest);

                return Ok(registrationRequestDto);

                //return CreatedAtRoute("GetRegistrationRequest", new { Id = registrationRequestDto.Id }, registrationRequestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing Add Registration Request");
                _logger.LogDebug($"Add Registration Request error - {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
