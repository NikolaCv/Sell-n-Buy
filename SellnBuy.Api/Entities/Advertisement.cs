using System.ComponentModel.DataAnnotations;

namespace SellnBuy.Api.Entities;

public class Advertisement : BaseEntity
{
	[Required]
	[StringLength(50)]
	public required string Title { get; set; }

	public string? Description { get; set; }

	[Required]
	[Range(0, 10e8)]
	public decimal Price { get; set; }

	[Required]
	public int ConditionId { get; set; }

	[Required]
	public int UserId { get; set; }

	[Required]
	public int CategoryId { get; set; }
}
