using AutoMapper;
using CommunityHub.Api.NamedConstants;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Services.Registration;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.Api.Controllers
{
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly IMapper _mapper;
        private readonly IRegistrationService _registrationService;

        public RegistrationController(
            ILogger<RegistrationController> logger,
            IMapper mapper,
            IRegistrationService registrationService)
        {
            _logger = logger;
            _mapper = mapper;

            _registrationService = registrationService;
        }

        [HttpPost(ApiRoute.Registration.CreateRequest)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegistrationRequestDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<RegistrationRequestDto>> AddRegistrationRequest([FromBody] RegistrationInfoCreateDto registrationInfoDto)
        {
            if (registrationInfoDto == null || !ModelState.IsValid)
            {
                return BadRequest(ErrorMessages.Registration.InvalidCreateData);
            }

            try
            {
                var registrationInfo = _mapper.Map<RegistrationInfo>(registrationInfoDto);

                var registrationRequest = await _registrationService.CreateRequestAsync(registrationInfo);
                var registrationRequestDto = _mapper.Map<RegistrationRequestDto>(registrationRequest);

                return CreatedAtRoute(RouteNames.Registration.GetRequest, new { id = registrationRequestDto.Id }, registrationRequestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogError.Registration.AddRequestProcessingError);
                _logger.LogError(String.Format(LogError.Registration.AdRequestErrorInfo, ex.Message));

                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.Common.RequestProcessingError);
            }
        }

        [HttpGet(ApiRoute.Registration.GetRequestById, Name = RouteNames.Registration.GetRequest)]
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
    }
}
