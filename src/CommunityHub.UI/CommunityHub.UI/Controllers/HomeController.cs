using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CommunityHub.UI.Models;
using CommunityHub.Core.Dtos;
using CommunityHub.UI.Services;

namespace CommunityHub.UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBaseService _service;

    public HomeController(ILogger<HomeController> logger, IBaseService baseService)
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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
