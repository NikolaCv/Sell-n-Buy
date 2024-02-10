using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Data;

public class SellnBuyContext : DbContext
{
	public SellnBuyContext(DbContextOptions<SellnBuyContext> options)
		: base(options)
	{
	}

	public DbSet<User> Users => Set<User>();
	public DbSet<Advertisement> Advertisements => Set<Advertisement>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
