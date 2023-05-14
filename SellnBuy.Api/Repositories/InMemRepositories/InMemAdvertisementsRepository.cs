using SellnBuy.Api.Entities;
using SellnBuy.Api.Enums;

namespace SellnBuy.Api.Repositories.InMemRepositories
{
	public class InMemAdvertisementsRepository : InMemRepository<Advertisement>
	{
		public InMemAdvertisementsRepository() : base(new List<Advertisement>
		{
			new Advertisement
			{
				Id = Guid.NewGuid(),
				Title = "pezo",
				Description = "ko nov",
				Price = 3500,
				Condition = Condition.Used,
				CreatedDate = DateTimeOffset.UtcNow,
				UserId = Guid.NewGuid(),
				CategoryId = Guid.NewGuid()
			},
			new Advertisement
			{
				Id = Guid.NewGuid(),
				Title = "scaffolding",
				Description = "stara al' dobra",
				Price = 4000,
				Condition = Condition.Used,
				CreatedDate = DateTimeOffset.UtcNow,
				UserId = Guid.NewGuid(),
				CategoryId = Guid.NewGuid()
			}
		})
		{
		}
	}
}