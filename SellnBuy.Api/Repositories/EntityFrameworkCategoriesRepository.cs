using SellnBuy.Api.Data;
using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Repositories;

public class EntityFrameworkCategoriesRepository : EntityFrameworkBaseRepository<Category>
{
    public EntityFrameworkCategoriesRepository(SellnBuyContext dbContext) : base(dbContext)
    {
    }
}