using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Data;

public class SellnBuyContext : IdentityDbContext<User, IdentityRole, string>
{
	public SellnBuyContext(DbContextOptions<SellnBuyContext> options)
		: base(options)
	{
	}

	public DbSet<Advertisement> Advertisements => Set<Advertisement>();
	public DbSet<Condition> Conditions => Set<Condition>();
	public DbSet<Category> Categories => Set<Category>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		modelBuilder.Entity<IdentityRole>().HasData(
			new IdentityRole { Name = "User", NormalizedName = "USER" },
			new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" }
		);

		modelBuilder.Entity<IdentityUserLogin<string>>()
			.HasKey(l => new { l.LoginProvider, l.ProviderKey });
		modelBuilder.Entity<IdentityUserRole<string>>()
 			.HasKey(r => new { r.UserId, r.RoleId });
		modelBuilder.Entity<IdentityUserToken<string>>()
			.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

		modelBuilder.Entity<Condition>()
			.HasIndex(c => c.Name)
			.IsUnique();
		modelBuilder.Entity<Condition>()
			.Property(c => c.Name)
			.IsRequired();

		modelBuilder.Entity<Category>()
			.HasIndex(c => c.Name)
			.IsUnique();
		modelBuilder.Entity<Category>()
			.Property(c => c.Name)
			.IsRequired();

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
