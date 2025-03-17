using AppComponents.Repository.Abstraction;
using AutoMapper;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Services.User;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(List<UserInfoDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<UserInfoDto>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            if(!users.Any())
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<UserInfoDto>>(users));
        }
    }
}
