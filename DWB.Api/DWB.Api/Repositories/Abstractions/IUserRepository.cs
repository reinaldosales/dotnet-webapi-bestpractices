using DWB.Api.Entities;
using DWB.Api.Models;

namespace DWB.Api.Repositories.Abstractions;

public interface IUserRepository
{
    Task<User> Create(CreateUserRequest model);
    Task<IEnumerable<User>> GetAll(int pageIndex, int pageSize);
    Task<User> GetById(Guid id);
    Task<User> GetByUsername(string username);
}
