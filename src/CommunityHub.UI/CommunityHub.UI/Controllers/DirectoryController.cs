using CommunityHub.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommunityHub.UI.Controllers
{
    [ApiController]
    [Route("directory")]
    public class DirectoryController : ControllerBase
    {
        private readonly ILogger<DirectoryController> _logger;
        private readonly IBaseService _service;

        public DirectoryController(ILogger<DirectoryController> logger, IBaseService baseService)
        {
            _logger = logger;
            _service = baseService;       
        }

        public async Task<IActionResult> Index()
        {
            var users = await _service.GetRequestAsync("api/users")
            return View(users);
        }
    }
}
