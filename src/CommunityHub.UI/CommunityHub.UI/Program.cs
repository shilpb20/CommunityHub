using CommunityHub.UI.Services.Registration;
using CommunityHub.UI;
using CommunityHub.UI.Services;
using Microsoft.Extensions.Options;
using CommunityHub.Core.Factory;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var apiSettings = builder.Configuration.GetSection("ApiSettings");
builder.Services.Configure<ApiSettings>(apiSettings);
builder.Services.AddHttpClient<BaseService>((sp, client) =>
{
    var apiSettings = sp.GetRequiredService<IOptions<ApiSettings>>();
    client.BaseAddress = new Uri(apiSettings.Value.BaseUrl);
});

builder.Services.AddScoped<BaseService>((sp) =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    var apiSettings = sp.GetRequiredService<IOptions<ApiSettings>>();
    var requestSender = sp.GetRequiredService<IHttpRequestSender>();

    return new BaseService(httpClient, requestSender, apiSettings);
});

builder.Services.AddScoped<IResponseFactory, ResponseFactory>();
builder.Services.AddScoped<IHttpRequestSender, HttpRequestSender>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IAdminService, AdminService>();

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
    pattern: "{controller=Account}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
