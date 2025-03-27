using AutoMapper;
using CommunityHub.Core;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Factory;
using CommunityHub.Core.Messages;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Services.AdminService;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;

namespace CommunityHub.Api.Controllers
{
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IMapper _mapper;
        private readonly IResponseFactory _responseFactory;
        private readonly IAdminService _adminService;

        public AdminController(
            ILogger<AdminController> logger,
            IMapper mapper,
            IResponseFactory responseFactory,
            IAdminService adminService)
        {
            _logger = logger;
            _mapper = mapper;
            _responseFactory = responseFactory;

            _adminService = adminService;
        }


        [HttpPut(ApiRoute.Admin.RejectRequestById)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationRequestDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<RegistrationRequestDto>>> RejectRequest(int id, [FromBody]string comment)
        {
            try
            {
                if (id <= 0) return BadRequest(_responseFactory.Failure<RegistrationRequestDto>(ErrorCode.InvalidData, ErrorMessage.InvalidId));

                var registrationRequest = await _adminService.RejectRequestAsync(id, comment);
                if (registrationRequest == null) return BadRequest(_responseFactory.Failure<RegistrationRequestDto>(ErrorCode.InvalidData, 
                    $"Request with ID {id} not found or could not be rejected."));

                var responseObject = _responseFactory.Success(_mapper.Map<RegistrationRequestDto>(registrationRequest));
                return Ok(responseObject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while rejecting request - {id}", id);
                return StatusCode(500, $"Unexpected error occurred while rejecting request - {id}");
            }
        }

        //[HttpPost(ApiRoute.Admin.ApproveRequestById)]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInfoDto))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<UserInfoDto>> ApproveRequest(int id)
        //{
        //    if (id <= 0) return BadRequest("Id must be a positive number.");

        //    var userInfo = await _registrationService.ApproveRequestAsync(id);
        //    if (userInfo == null) return NotFound($"Request with ID {id} not found or could not be approved.");

        //    return Ok(_mapper.Map<UserInfoDto>(userInfo));
        //}
    }
}
