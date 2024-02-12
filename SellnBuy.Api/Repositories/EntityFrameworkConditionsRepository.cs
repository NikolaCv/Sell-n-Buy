using SellnBuy.Api.Data;
using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Repositories;

public class EntityFrameworkConditionsRepository : EntityFrameworkBaseRepository<Condition>
{
	public EntityFrameworkConditionsRepository(SellnBuyContext dbContext) : base(dbContext)
	{
	}
}