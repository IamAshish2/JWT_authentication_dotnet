using JWT_AUTHENTICATION.Data;
using JWT_AUTHENTICATION.Models;
using JWT_AUTHENTICATION.Services.PasswordHasher;
using JWT_AUTHENTICATION.Services.TokenGenerator;
using JWT_AUTHENTICATION.Services.UserRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Access the configuration object
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// Bind AuthenticationConfiguration from appsettings.json
AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
configuration.Bind("Jwt", authenticationConfiguration);
builder.Services.AddSingleton(authenticationConfiguration);

builder.Services.AddScoped<IPasswordHasher,BcryptPasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AccessTokenGenerator>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(
        JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser() 
            .Build();
});

// configuring dbContext with appDbcontext to get the connection string for database from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


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