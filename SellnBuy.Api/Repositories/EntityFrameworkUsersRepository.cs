using SellnBuy.Api.Entities;
using Microsoft.EntityFrameworkCore;
using SellnBuy.Api.Data;

namespace SellnBuy.Api.Repositories;

public class EntityFrameworkUsersRepository : IUsersRepository
{
	private readonly SellnBuyContext dbContext;

	public EntityFrameworkUsersRepository(SellnBuyContext dbContext)
	{
		this.dbContext = dbContext;
	}

	// public async Task CreateAsync(User user)
	// {
	// 	await dbContext.Users.AddAsync(user);
	// 	await dbContext.SaveChangesAsync();
	// }

	public async Task DeleteAsync(string id)
	{
		await dbContext.Users.Where(user => user.Id == id)
						.ExecuteDeleteAsync();
	}

	public virtual async Task<IEnumerable<User>> GetAllAsync()
	{
		return await dbContext.Users.ToListAsync();
	}

	public virtual async Task<User?> GetAsync(string id)
	{
		return await dbContext.Users.FindAsync(id);
	}

	public async Task UpdateAsync(User user)
	{
		dbContext.Update(user);
		await dbContext.SaveChangesAsync();
	}
}