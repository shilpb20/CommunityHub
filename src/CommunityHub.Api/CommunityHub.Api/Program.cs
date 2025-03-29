using AppComponents.Email;
using AppComponents.Repository.EFCore;
using AppComponents.TemplateEngine;
using CommunityHub.Api.Data;
using CommunityHub.Core.Factory;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.EmailService;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Services;
using CommunityHub.Infrastructure.Services.Account;
using CommunityHub.Infrastructure.Services.AdminService;
using CommunityHub.Infrastructure.Services.Registration;
using CommunityHub.Infrastructure.Services.User;
using CommunityHub.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);

var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
builder.Services.AddEmailService(emailSettings);

builder.Services.AddScoped<IModelTemplateEngine>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<IModelTemplateEngine>>();
    return new ModelTemplateEngine(logger, "{{", "}}");
});


builder.Services.AddScoped<IAppMailService, AppMailService>();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
        };
    });
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 6;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(24);
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
});


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
builder.Services.AddRepository<Child, ApplicationDbContext>();
builder.Services.AddRepository<FamilyPicture, ApplicationDbContext>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddRepository<RegistrationRequest, ApplicationDbContext>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ISpouseService, SpouseService>();
builder.Services.AddScoped<IChildService, ChildService>();
builder.Services.AddScoped<IUserInfoValidationService, UserInfoValidatorService>();
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
