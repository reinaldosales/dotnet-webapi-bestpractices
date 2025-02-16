using DWB.Api.Context;
using DWB.Api.Entities;
using DWB.Api.Models;
using DWB.Api.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DWB.Api.Repositories;

public class UserRepository(AppDbContext appDbContext) : IUserRepository
{
    public AppDbContext Context { get; } = appDbContext;

    public async Task<User> Create(CreateUserRequest model, CancellationToken cancellationToken = default)
    {

        var user = User.CreateUser(model.Username, model.Password);

        await Context.Users.AddAsync(user, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<IEnumerable<User>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        => await Context.Users
        .AsNoTracking()
        .Skip(pageIndex * pageSize)
        .Take(pageSize)
        .ToListAsync(cancellationToken);

    public async Task<User> GetById(Guid id, CancellationToken cancellationToken = default)
        => await Context.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<User> GetByUsername(string username, CancellationToken cancellationToken = default)
        => await Context.Users
        .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
}
