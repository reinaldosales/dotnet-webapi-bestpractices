using System.ComponentModel.DataAnnotations;

namespace DWB.Api.Models;

public class CreateUserModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}
