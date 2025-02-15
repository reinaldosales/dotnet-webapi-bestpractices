using System.ComponentModel.DataAnnotations;

namespace DWB.Api.Models;

public class CreateUserRequest
{
    public string Username { get; set; }

    public string Password { get; set; }
}
