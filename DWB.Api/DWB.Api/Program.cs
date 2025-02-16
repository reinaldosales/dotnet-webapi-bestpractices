using DWB.Api.Context;
using DWB.Api.Models;
using DWB.Api.Repositories;
using DWB.Api.Repositories.Abstractions;
using DWB.Api.Service;
using DWB.Api.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAllElasticApm();

#pragma warning disable EXTEXP0018 // O tipo � apenas para fins de avalia��o e est� sujeito a altera��es ou remo��o em atualiza��es futuras. Suprima este diagn�stico para continuar.
builder.Services.AddHybridCache();
#pragma warning restore EXTEXP0018 // O tipo � apenas para fins de avalia��o e est� sujeito a altera��es ou remo��o em atualiza��es futuras. Suprima este diagn�stico para continuar.

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
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.UseSwaggerUI();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("api/v1/authentication", async (string username, IUserRepository userRepository, CancellationToken cancellationToken) =>
{
    var user = await userRepository.GetByUsername(username, cancellationToken);

    if (user is not null)
        return Results.Ok(JwtBearerService.GenerateToken(user, secretKey));

    return Results.Unauthorized();
});

app.MapPost("api/v1/user", async (CreateUserRequest model, IUserRepository userRepository, HybridCache cache, CancellationToken cancellationToken) =>
{
    var validator = new CreateUserValidator();

    var result = await validator.ValidateAsync(model, cancellationToken);

    if (!result.IsValid)
        return Results.ValidationProblem(result.ToDictionary());

    var user = await userRepository.Create(model, cancellationToken);

    await cache.SetAsync($"user-{model.Username}", user, cancellationToken: cancellationToken);

    return Results.Created();
})
.WithName("CreateUser")
.WithOpenApi();

app.MapGet("api/v1/user", async (IUserRepository userRepository, CancellationToken cancellationToken, int pageIndex = 0, int pageSize = 20) =>
{
    var users = await userRepository.GetAll(pageIndex, pageSize, cancellationToken);

    return Results.Ok(users);
})
.RequireAuthorization()
.WithName("GetAll")
.WithOpenApi();

app.MapGet("api/v1/user/{username}", async (string username, IUserRepository userRepository, HybridCache cache, CancellationToken cancellationToken) =>
{
    var user = await cache.GetOrCreateAsync(
        $"user-{username}",
        async cancel => await userRepository.GetByUsername(username, cancel),
        cancellationToken: cancellationToken
    );

    if (user is not null)
        return Results.Ok(user);

    return Results.NotFound();
})
.RequireAuthorization()
.WithName("GetById")
.WithOpenApi();

app.Run();
