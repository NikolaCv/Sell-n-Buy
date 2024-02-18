using Microsoft.EntityFrameworkCore;
using SellnBuy.Api.Data;
using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Repositories;

public class EntityFrameworkBaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
	protected readonly SellnBuyContext dbContext;

	public EntityFrameworkBaseRepository(SellnBuyContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task CreateAsync(T item)
	{
		await dbContext.Set<T>().AddAsync(item);
		await dbContext.SaveChangesAsync();
	}

	public async Task DeleteAsync(int id)
	{
		await dbContext.Set<T>().Where(item => item.Id == id)
						.ExecuteDeleteAsync();
	}

	public virtual async Task<IEnumerable<T>> GetAllAsync()
	{
		return await dbContext.Set<T>().AsNoTracking().ToListAsync();
	}

	public virtual async Task<T?> GetAsync(int id)
	{
		return await dbContext.Set<T>().FindAsync(id);
	}

	public async Task UpdateAsync(T item)
	{
		dbContext.Update(item);
		await dbContext.SaveChangesAsync();
	}
}