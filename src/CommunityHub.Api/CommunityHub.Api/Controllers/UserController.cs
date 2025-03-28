using AutoMapper;
using CommunityHub.Core;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Factory;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.Api.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IResponseFactory _responseFactory;

        private readonly IUserService _userService;

        public UserController(
            ILogger<UserController> logger,
            IMapper mapper,
            IResponseFactory responseFactory,
            IUserService userService)
        {
            _logger = logger;
            _mapper = mapper;
            _responseFactory = responseFactory;

            _userService = userService;
        }


        [HttpGet(ApiRoute.Users.GetAll)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<UserInfoDto>>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ApiResponse<List<UserInfoDto>>>> GetUsers(
            [FromQuery] string? sortBy,
            [FromQuery] bool ascending = true)
        {
            var users = await _userService.GetUsersAsync(sortBy, ascending);
            if (!users.Any())
            {
                return NoContent();
            }

            var responseObject = _responseFactory.Success(_mapper.Map<List<UserInfoDto>>(users));
            return Ok(responseObject);
        }
    }
}