using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AppComponents.Repository.Abstraction;
using AppComponents.Repository.EFCore;
using AppComponents.Repository.EFCore.Transaction;
using AppComponents.Email;
using CommunityHub.Api.Data;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Services;
using CommunityHub.Infrastructure.Services.Registration;
using AppComponents.Email.Services;
using CommunityHub.Infrastructure.Services.User;
using AppComponents.TemplateEngine;
using CommunityHub.Infrastructure.EmailService;
using CommunityHub.Api;
using Microsoft.Extensions.Options;
using CommunityHub.Core.Factory;
using CommunityHub.Infrastructure.Services.AdminService;

var builder = WebApplication.CreateBuilder(args);

// Bind the "AppSettings" section (for general app settings) and register it as a singleton.
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);

// Bind the "EmailAppSettings" section (for email-specific settings) and register it as a singleton.
var emailAppSettingsSection = builder.Configuration.GetSection("EmailAppSettings");
builder.Services.Configure<EmailAppSettings>(emailAppSettingsSection);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<EmailAppSettings>>().Value);

// Email-related services
var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
builder.Services.AddEmailService(emailSettings);

builder.Services.AddScoped<IModelTemplateEngine>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<IModelTemplateEngine>>();
    return new ModelTemplateEngine(logger, "{{", "}}");
});


builder.Services.AddScoped<IAppMailService, AppMailService>();

// Database setup
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var appSettings = appSettingsSection.Get<AppSettings>();
bool useInMemoryDb = appSettings.TransactionSettings.UseInMemoryDatabase;

if (useInMemoryDb)
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("TestDB"));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddTransactionManager<ApplicationDbContext>();

// Repositories and other services
builder.Services.AddRepository<UserInfo, ApplicationDbContext>();
builder.Services.AddRepository<SpouseInfo, ApplicationDbContext>();
builder.Services.AddRepository<Children, ApplicationDbContext>();
builder.Services.AddRepository<FamilyPicture, ApplicationDbContext>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddRepository<RegistrationRequest, ApplicationDbContext>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IResponseFactory, ResponseFactory>();

// Controllers, API setup, and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// Configure the app to explicitly use HTTP/HTTPS ports
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

var app = builder.Build();

// Seed roles
var serviceProvider = app.Services.CreateScope().ServiceProvider;
await DataSeeder.SeedRolesAsync(serviceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
