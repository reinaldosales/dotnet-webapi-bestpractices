using DWB.Api.Context;
using DWB.Api.Models;
using DWB.Api.Repositories;
using DWB.Api.Repositories.Abstractions;
using DWB.Api.Service;
using DWB.Api.Utils;
using DWB.Api.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAllElasticApm();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("api"));

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new(1.0);
    options.AssumeDefaultVersionWhenUnspecified = true;
});

var secretKey = builder.Configuration.GetSection("SecretKey")?.Value;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("api/v1/authentication", async (string username, IUserRepository userRepository) =>
{
    var user = await userRepository.GetByUsername(username);

    if (user is not null)
        return Results.Ok(JwtBearerService.GenerateToken(user, secretKey));

    return Results.Unauthorized();
});

app.MapPost("api/v1/user", async (CreateUserRequest model, IUserRepository userRepository) =>
{
    var validator = new CreateUserValidator();

    var result = await validator.ValidateAsync(model);

    if (!result.IsValid)
        return Results.ValidationProblem(result.ToDictionary());

    var user = await userRepository.Create(model);

    return Results.Created();
})
.WithName("CreateUser")
.WithOpenApi();

app.MapGet("api/v1/user", async (IUserRepository userRepository, int pageIndex = 0, int pageSize = 20) =>
{
    var users = await userRepository.GetAll(pageIndex, pageSize);

    return Results.Ok(users);
})
.RequireAuthorization()
.WithName("GetAll")
.WithOpenApi();

app.MapGet("api/v1/user/{username}", async (string username, IUserRepository userRepository) =>
{
    var user = await userRepository.GetByUsername(username);

    if(user is not null)
        return Results.Ok(user);

    return Results.NotFound();
})
.RequireAuthorization()
.WithName("GetById")
.WithOpenApi();


app.Run();
