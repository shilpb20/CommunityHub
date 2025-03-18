using CommunityHub.Core.Dtos;
using CommunityHub.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityHub.UI.Controllers
{
    [ApiController]
    [Route("directory")]
    public class DirectoryController : Controller
    {
        private readonly ILogger<DirectoryController> _logger;
        private readonly IBaseService _service;

        public DirectoryController(ILogger<DirectoryController> logger, IBaseService baseService)
        {
            _logger = logger;
            _service = baseService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? sortBy = null, bool ascending = true)
        {
            ViewBag.SelectedSortBy = sortBy;
            ViewBag.SelectedAscending = ascending;

            var users = await FetchUsersAsync(sortBy, ascending);
            return View(users);
        }

        private async Task<List<UserInfoDto>> FetchUsersAsync(string? sortBy, bool ascending)
        {
            string uri = "api/users";
            if (!string.IsNullOrEmpty(sortBy))
            {
                uri += $"?sortBy={sortBy}&ascending={ascending}";
            }

            var users = await _service.GetRequestAsync<List<UserInfoDto>>(uri);
            return users ?? new List<UserInfoDto>();
        }
    }
}