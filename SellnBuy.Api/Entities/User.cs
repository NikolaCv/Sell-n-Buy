namespace SellnBuy.Api.Entities
{
	public class User : BaseEntity
	{
		public string Name { get; set; }
		public string Bio { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
	}
}