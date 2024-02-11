using SellnBuy.Api.Data;
using SellnBuy.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace SellnBuy.Api.Repositories;

public class EntityFrameworkConditionsRepository : EntityFrameworkBaseRepository<Condition>
{
	public EntityFrameworkConditionsRepository(SellnBuyContext dbContext) : base(dbContext)
	{
	}
}