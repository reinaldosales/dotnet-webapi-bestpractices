using DWB.Api.Context;
using DWB.Api.Entities;
using DWB.Api.Models;
using DWB.Api.Repositories.Abstractions;

namespace DWB.Api.Repositories;

public class UserRepository(AppDbContext appDbContext) : IUserRepository
{
    public AppDbContext _context { get; } = appDbContext;

    public async Task<User> Create(CreateUserModel model)
    {

        var user = User.CreateUser(model.Username, model.Password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }
}
