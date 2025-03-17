using CommunityHub.Core.Dtos;
using CommunityHub.UI.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index()
        {
            var users = await _service.GetRequestAsync<List<UserInfoDto>>("api/users");
            return View(users);
        }
    }
}
