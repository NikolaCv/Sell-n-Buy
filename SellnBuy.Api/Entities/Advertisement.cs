using System.ComponentModel.DataAnnotations;

namespace SellnBuy.Api.Entities;

public class Advertisement : BaseEntity
{
	[StringLength(50)] public required string Title { get; set; }
	public string? Description { get; set; }
	[Range(1, 10e8)] public decimal Price { get; set; }
	public int ConditionId { get; set; }
	public Condition? Condition { get; set; }
	public int UserId { get; set; }
	public User? User { get; set; }
	public int CategoryId { get; set; }
	public Category? Category { get; set; }
}
