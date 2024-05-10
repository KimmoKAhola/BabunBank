using System.ComponentModel.DataAnnotations;
using BabunBank.Infrastructure.Configurations.CustomAnnotations;

namespace BabunBank.Models.FormModels.Customer;

public record DeleteCustomerModel
{
    [Required]
    public string FirstName { get; init; } = null!;

    [Required]
    public string Surname { get; init; } = null!;

    [Required]
    public int Id { get; init; }

    [Compare("Id", ErrorMessage = "Id has to match")]
    public int ConfirmId { get; init; }

    [CompareFullName(ErrorMessage = "Name should match the customer's name.")]
    public string ConfirmFullName { get; init; } = null!;
}
