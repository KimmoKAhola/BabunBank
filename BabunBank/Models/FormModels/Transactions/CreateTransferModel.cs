using System.ComponentModel.DataAnnotations;
using BabunBank.Infrastructure.Configurations.CustomAnnotations;

namespace BabunBank.Models.FormModels.Transactions;

public record CreateTransferModel
{
    [Required]
    [Range(1, int.MaxValue)]
    public int FromAccountId { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int FromCustomerId { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ToAccountId { get; init; }

    [Required]
    [Range(0.01, 100000)]
    [MaximumTransferAmount(nameof(BalanceSender))]
    public decimal Amount { get; init; }

    [Required]
    [DateTransactionCheck]
    public DateOnly Date { get; init; } = DateOnly.FromDateTime(DateTime.Now);

    [Required]
    public string Type { get; init; } = "Debit";

    [Required]
    public string OperationSender { get; init; } = "Transfer to";

    [Required]
    public string OperationReceiver { get; init; } = "Transfer from";

    [Required]
    public decimal BalanceSender { get; init; }

    [Required]
    public decimal BalanceReceiver { get; init; }
    public string? Symbol { get; init; }
    public string? Bank { get; init; }
    public string? Account { get; init; }
}
