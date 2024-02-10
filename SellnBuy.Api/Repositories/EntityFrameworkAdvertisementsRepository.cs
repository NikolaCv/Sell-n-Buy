using SellnBuy.Api.Data;
using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Repositories
{
    public class EntityFrameworkAdvertisementsRepository : EntityFrameworkBaseRepository<Advertisement>
    {
        public EntityFrameworkAdvertisementsRepository(SellnBuyContext dbContext) : base(dbContext)
        {
        }
    }
}