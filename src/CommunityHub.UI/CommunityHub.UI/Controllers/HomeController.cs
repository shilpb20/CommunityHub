using CommunityHub.Core.Constants;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Factory;
using CommunityHub.Core.Models;
using CommunityHub.UI.Constants;
using CommunityHub.UI.Models;
using CommunityHub.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CommunityHub.UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BaseService _service;
    private readonly IResponseFactory _responseFactory;

    public HomeController(ILogger<HomeController> logger, 
       IResponseFactory responseFactory,
       BaseService baseService)
    {
        _logger = logger;
        _service = baseService;
        _responseFactory = responseFactory;
    }

    [HttpGet(UiRoute.Home.Index)]
    public async Task<IActionResult> Index(string? sortBy = null, bool ascending = true)
    {
        string uri = ApiRoute.Users.GetAll;
        if (!string.IsNullOrEmpty(sortBy))
        {
            uri += $"?sortBy={sortBy}&ascending={ascending}";
        }

        ApiResponse<List<UserInfoDto>> users = await _service.GetRequestAsync<List<UserInfoDto>>(uri);

        ViewBag.SelectedSortBy = sortBy;
        ViewBag.SelectedAscending = ascending;
        return View(users.Data);
    }

    [HttpGet(UiRoute.Home.Privacy)]
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
