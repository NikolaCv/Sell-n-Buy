using Microsoft.EntityFrameworkCore;
using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Data.Configurations;

public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
{
	public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Advertisement> builder)
	{
		builder.Property(advertisement => advertisement.Price)
				.HasPrecision(10, 2);
	}
}