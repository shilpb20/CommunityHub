 using AppComponents.Repository.Abstraction;
using AutoMapper;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(
            ILogger<UserController> logger,
            IMapper mapper,
            IUserService userService)
        {
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserInfoDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<UserInfoDto>>> GetUsers(
            [FromQuery]string? sortBy,
            [FromQuery]bool ascending = true)
        {
            Dictionary<string, bool> orderBy = new();
            if(sortBy == null)
            {
                orderBy = new Dictionary<string, bool>
                {
                    { "Location", true },
                    { "FullName", true }
                };
            }
            else
            {
                orderBy = new Dictionary<string, bool>()
                {
                    { sortBy, ascending }
                };
            }

            var users = await _userService.GetUsersAsync(orderBy);
            if (!users.Any())
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<UserInfoDto>>(users));
        }
    }
}