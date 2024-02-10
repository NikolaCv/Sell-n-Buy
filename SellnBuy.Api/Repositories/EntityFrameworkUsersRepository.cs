using SellnBuy.Api.Data;
using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Repositories
{
    public class EntityFrameworkUsersRepository : EntityFrameworkBaseRepository<User>
    {
        public EntityFrameworkUsersRepository(SellnBuyContext dbContext) : base(dbContext)
        {
        }
    }
}