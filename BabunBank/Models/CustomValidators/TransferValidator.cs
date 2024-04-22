using System.Data;
using BabunBank.Models.FormModels.TransferModels;
using FluentValidation;

namespace BabunBank.Models.CustomValidators;

public class TransferValidator : AbstractValidator<CreateDepositModel>
{
    public TransferValidator()
    {
        RuleFor(deposit => deposit.AccountId).NotNull();

        RuleFor(deposit => deposit.Date);

        RuleFor(deposit => deposit.Type);

        RuleFor(deposit => deposit.Operation);

        RuleFor(deposit => deposit.Amount)
            .NotNull()
            .WithMessage("Deposit amount is required.")
            .InclusiveBetween(0.01m, 100000m)
            .WithMessage("Deposit amount must be between 0.01 and 100000")
            .PrecisionScale(8, 2, true)
            .WithMessage("You can at most have 2 decimals.");

        RuleFor(deposit => deposit.Balance);

        // RuleFor();
    }
}
