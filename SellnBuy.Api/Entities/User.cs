using System.ComponentModel.DataAnnotations;
namespace SellnBuy.Api.Entities;

public class User : BaseEntity
{
	[StringLength(50)] public required string Name { get; set; }
	public string? Bio { get; set; }
	public string? PhoneNumber { get; set; }
	[StringLength(50)] public required string Email { get; set; }
}
