using SellnBuy.Api.Enums;

namespace SellnBuy.Api.Entities
{
	public class Advertisement : BaseEntity
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public Condition Condition {get; set;}
		public decimal Price { get; set; }
		public Guid userId { get; set; }
		public Guid categoryId { get; set; }
	}
}