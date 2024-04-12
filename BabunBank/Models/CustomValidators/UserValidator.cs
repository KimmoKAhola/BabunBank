using BabunBank.Models.FormModels.User;
using BabunBank.Services;
using DataAccessLibrary.Data;
using FluentValidation;

namespace BabunBank.Models.CustomValidators;

public class UserValidator : AbstractValidator<SignUpUserModel>
{
    private readonly IdentityUserService _identityUserService;

    public UserValidator(IdentityUserService identityUserService)
    {
        _identityUserService = identityUserService;

        RuleFor(user => user.UserName)
            .NotNull()
            .WithMessage("Username is required")
            .NotEmpty()
            .WithMessage("Username is required")
            .Length(6, 100)
            .WithMessage("Username must be between 2 and 100 characters")
            .MustAsync(BeUniqueUserName)
            .WithMessage("A user with that username already exists");

        RuleFor(user => user.Email)
            .NotNull()
            .WithMessage("Field can not be empty.")
            .NotEmpty()
            .WithMessage("Field can not be empty.")
            .Length(6, 100)
            .WithMessage("Email has to be at least 6 characters")
            .MustAsync(BeUniqueEmail)
            .WithMessage("A user with that email already exists.");

        RuleFor(user => user.Password).NotNull().NotEmpty();

        RuleFor(user => user.UserRole).NotNull().NotEmpty();
    }

    private async Task<bool> BeUniqueUserName(string username, CancellationToken cancellationToken)
    {
        return await _identityUserService.CheckIfExistsByUsernameAsync(username);
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        return await _identityUserService.CheckIfExistsByEmailAsync(email);
    }
}
