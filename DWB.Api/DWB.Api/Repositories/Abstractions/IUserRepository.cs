using DWB.Api.Entities;
using DWB.Api.Models;

namespace DWB.Api.Repositories.Abstractions;

public interface IUserRepository
{
    Task<User> Create(CreateUserModel model);
}
