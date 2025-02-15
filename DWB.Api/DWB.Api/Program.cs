using Asp.Versioning;
using DWB.Api.Context;
using DWB.Api.Models;
using DWB.Api.Repositories;
using DWB.Api.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("api"));

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new(1.0);
    options.AssumeDefaultVersionWhenUnspecified = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("api/v1/user", async (CreateUserModel model, IUserRepository userRepository) =>
{
    var user = await userRepository.Create(model);

    return Results.Created();
})
.WithName("CreateUser")
.WithOpenApi();

app.Run();
