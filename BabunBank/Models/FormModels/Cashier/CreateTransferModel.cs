using System.ComponentModel.DataAnnotations;

namespace BabunBank.Models.FormModels.Cashier;

public class CreateTransferModel
{
    [Required]
    public int FromAccountId { get; init; }

    [Required]
    public int ToAccountId { get; init; }

    [Required]
    [Range(1, 100000)]
    public decimal Amount { get; init; }

    [Required]
    public DateOnly Date { get; init; } = DateOnly.FromDateTime(DateTime.Now);

    [Required]
    public string Type { get; init; } = "Debit";

    [Required]
    public string OperationSender { get; init; } = "Transfer to";

    [Required]
    public string OperationReceiver { get; init; } = "Transfer from";

    [Required]
    public decimal Balance { get; init; }
    public string? Symbol { get; init; }
    public string? Bank { get; init; }
    public string? Account { get; init; }
}
