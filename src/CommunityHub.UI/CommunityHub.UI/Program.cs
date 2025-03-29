using CommunityHub.UI.Services.Registration;
using CommunityHub.UI;
using CommunityHub.UI.Services;
using Microsoft.Extensions.Options;
using CommunityHub.Core.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var appSettings = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettings);
builder.Services.AddHttpClient<BaseService>((sp, client) =>
{
    var appSettings = sp.GetRequiredService<IOptions<AppSettings>>();
    client.BaseAddress = new Uri(appSettings.Value.ClientUrl);
});

builder.Services.AddScoped<BaseService>((sp) =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    var appSettings = sp.GetRequiredService<IOptions<AppSettings>>();
    var requestSender = sp.GetRequiredService<IHttpRequestSender>();

    return new BaseService(httpClient, requestSender, appSettings);
});

builder.Services.AddScoped<IResponseFactory, ResponseFactory>();
builder.Services.AddScoped<IHttpRequestSender, HttpRequestSender>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
