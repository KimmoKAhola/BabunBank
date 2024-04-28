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

        RuleFor(user => user.Email)
            .NotNull()
            .WithMessage("Field can not be empty.")
            .NotEmpty()
            .WithMessage("Field can not be empty.")
            .Length(6, 100)
            .WithMessage("Email has to be at least 6 characters")
            .MustAsync(BeUniqueEmail)
            .WithMessage("A user with that email already exists.")
            .Equal("Email")
            .WithMessage("The email addresses has to match.");

        RuleFor(user => user.Password).NotNull().NotEmpty();

        RuleFor(user => (int)user.UserRole).NotEqual(0).WithMessage("Please choose a valid option");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var result = await _identityUserService.CheckIfExistsByEmailAsync(email);
        return result;
    }
}
