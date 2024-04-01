using Microsoft.Build.Framework;

namespace BabunBank.Models;

public class CreateCustomerModel{

    [Required]
    public string Name { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string AccountNumber { get; set; }
    [Required]
    public string Address { get; set; }
}