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
	public DbSet<Condition> Conditions => Set<Condition>();
	public DbSet<Category> Categories => Set<Category>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		modelBuilder.Entity<Condition>()
			.HasIndex(c => c.Name)
			.IsUnique();

		modelBuilder.Entity<Category>()
			.HasIndex(c => c.Name)
			.IsUnique();
			
		modelBuilder.Entity<Advertisement>()
			.HasOne(a => a.User)
			.WithMany()
			.HasForeignKey(a => a.UserId);

		modelBuilder.Entity<Advertisement>()
			.HasOne(a => a.Condition)
			.WithMany()
			.HasForeignKey(a => a.ConditionId);

   		 modelBuilder.Entity<Advertisement>()
			.HasOne(a => a.Category)
			.WithMany()
			.HasForeignKey(a => a.CategoryId);

	}
}
