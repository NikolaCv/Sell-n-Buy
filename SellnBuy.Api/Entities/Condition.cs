namespace SellnBuy.Api.Entities;

public class Condition : BaseEntity
{
	public required string Name { get; set; }
	public string? Description { get; set; }
}