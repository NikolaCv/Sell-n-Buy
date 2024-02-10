namespace SellnBuy.Api.Entities;

public abstract class BaseEntity
{
	public int Id { get; set; }
	public DateTimeOffset CreatedDate { get; set; }
}
