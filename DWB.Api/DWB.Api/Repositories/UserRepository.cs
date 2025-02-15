using DWB.Api.Context;
using DWB.Api.Entities;
using DWB.Api.Models;
using DWB.Api.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DWB.Api.Repositories;

public class UserRepository(AppDbContext appDbContext) : IUserRepository
{
    public AppDbContext _context { get; } = appDbContext;

    public async Task<User> Create(CreateUserRequest model)
    {

        var user = User.CreateUser(model.Username, model.Password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<IEnumerable<User>> GetAll()
        => await _context.Users.ToListAsync();

    public async Task<User> GetById(Guid id)
        => await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
}
