using AppComponents.Repository.Abstraction;
using AppComponents.Repository.EFCore;
using AppComponents.Repository.EFCore.Transaction;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Services.Registration;
using CommunityHub.Infrastructure.Services.User;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var transactionSettings = builder.Configuration.GetSection("TransactionSettings");
bool useInMemoryDb = transactionSettings.GetValue<bool>("UseInMemoryDatabase"); 
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

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<TransactionSettings>(builder.Configuration.GetSection("TransactionSettings"));
builder.Services.AddTransactionManager<ApplicationDbContext>();

builder.Services.AddRepository<RegistrationRequest, ApplicationDbContext>();
builder.Services.AddRepository<UserInfo, ApplicationDbContext>();
builder.Services.AddRepository<SpouseInfo, ApplicationDbContext>();
builder.Services.AddRepository<Children, ApplicationDbContext>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

public partial class Program
{

}
