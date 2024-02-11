using SellnBuy.Api.Data;
using SellnBuy.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace SellnBuy.Api.Repositories
{
	public class EntityFrameworkAdvertisementsRepository : EntityFrameworkBaseRepository<Advertisement>
	{
		public EntityFrameworkAdvertisementsRepository(SellnBuyContext dbContext) : base(dbContext)
		{
		}

		public override async Task<IEnumerable<Advertisement>> GetAllAsync()
		{
			return await dbContext.Advertisements
				.Include(a => a.User)
				.Include(a => a.Condition)
				.Include(a => a.Category)
				.AsNoTracking()
				.ToListAsync();
		}

		public override async Task<Advertisement?> GetAsync(int id)
		{
			return await dbContext.Advertisements
				.Include(a => a.User)
				.Include(a => a.Condition)
				.Include(a => a.Category)
				.FirstOrDefaultAsync(a => a.Id == id);
		}
	}
}