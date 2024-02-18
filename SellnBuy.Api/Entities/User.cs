using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace SellnBuy.Api.Entities;

public class User : IdentityUser
{
	[StringLength(50)] public required string Name { get; set; }
	public string? Bio { get; set; }
	public DateTimeOffset CreatedDate { get; set; }
}
