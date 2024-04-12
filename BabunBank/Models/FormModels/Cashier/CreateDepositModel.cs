using System.ComponentModel.DataAnnotations;

namespace BabunBank.Models.FormModels.Cashier;

public class CreateDepositModel
{
    [Required]
    public int AccountId { get; init; }

    [Required]
    public DateOnly Date { get; init; }

    [Required]
    public string Type { get; init; } = null!;

    [Required]
    public string Operation { get; init; } = null!;

    [Required]
    [Range(0.01, 100000)]
    public decimal Amount { get; init; }

    [Required]
    public decimal Balance { get; init; }
    public string? Symbol { get; init; }
    public string? Bank { get; init; }
    public string? Account { get; init; }
}
