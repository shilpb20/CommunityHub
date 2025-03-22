using AutoMapper;
using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Infrastructure.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.Api.Controllers
{
    [ApiController]
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


        [HttpGet(ApiRoute.Users.GetAll)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserInfoDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<UserInfoDto>>> GetUsers(
            [FromQuery] string? sortBy,
            [FromQuery] bool ascending = true)
        {
            Dictionary<string, bool> orderBy = new();
            if (sortBy == null)
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


        [HttpGet(ApiRoute.Users.Find)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInfoDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserInfoDto>> GetUser(
            [FromQuery] string? email,
            [FromQuery] string? contact,
            [FromQuery] string? spouseEmail,
            [FromQuery] string? spouseContact)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(contact) &&
                string.IsNullOrEmpty(spouseEmail) && string.IsNullOrEmpty(spouseContact))
            {
                return BadRequest("At least one of email, contact, spouseEmail, or spouseContact must be provided.");
            }

            var user = await _userService.GetUserAsync(
                u =>
                    // Check user email and contact
                    ((!string.IsNullOrEmpty(u.Email) && u.Email == email) ||
                     (!string.IsNullOrEmpty(u.ContactNumber) && u.ContactNumber == contact)) ||

                    // Check spouse email and contact
                    (u.SpouseInfo != null &&
                     ((!string.IsNullOrEmpty(u.SpouseInfo.Email) && u.SpouseInfo.Email == spouseEmail) ||
                      (!string.IsNullOrEmpty(u.SpouseInfo.ContactNumber) && u.SpouseInfo.ContactNumber == spouseContact)))
            );

            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserInfoDto>(user));
        }
    }
}