using DWB.Api.Models;
using FluentValidation;

namespace DWB.Api.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty();

        RuleFor(user => user.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}
