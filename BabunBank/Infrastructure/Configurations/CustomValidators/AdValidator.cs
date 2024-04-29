using BabunBank.Models.FormModels.Ad;
using FluentValidation;

namespace BabunBank.Infrastructure.Configurations.CustomValidators;

public class AdValidator : AbstractValidator<EditAdModel>
{
    public AdValidator()
    {
        RuleFor(ad => ad.Title)
            .NotEmpty()
            .WithMessage("Title can not be empty.")
            .MinimumLength(5)
            .WithMessage("Title has to be at least 5 characters")
            .MaximumLength(50)
            .WithMessage("Title has to be under 50 characters")
            .WithName("Title");

        RuleFor(ad => ad.Author)
            .NotEmpty()
            .WithMessage("Author name can not be empty")
            .MinimumLength(2)
            .WithMessage("Author name has to be at least 2 characters.")
            .MaximumLength(50)
            .WithMessage("Author name can be at most 50 characters")
            .Matches(@"^\D")
            .WithMessage("No numbers are allowed")
            .WithName("Author");

        RuleFor(ad => ad.Description)
            .NotEmpty()
            .WithMessage("Description can not be empty.")
            .MinimumLength(5)
            .WithMessage("Description has to be at least 5 characters")
            .MaximumLength(30)
            .WithMessage("Description has to be under 30 characters")
            .WithName("Description");

        RuleFor(ad => ad.Content)
            .NotEmpty()
            .WithMessage("Content can not be empty.")
            .MinimumLength(50)
            .WithMessage("Content has to be at least 50 characters")
            .MaximumLength(2000)
            .WithMessage("Content has to be under 2000 characters")
            .WithName("Content");
    }
}
