using DWB.Api.Entities;
using DWB.Api.Models;

namespace DWB.Api.Repositories.Abstractions;

public interface IUserRepository
{
    Task<User> Create(CreateUserRequest model, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
    Task<User> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<User> GetByUsername(string username, CancellationToken cancellationToken = default);
}
