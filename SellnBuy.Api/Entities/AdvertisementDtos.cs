using System.ComponentModel.DataAnnotations;
using SellnBuy.Api.Enums;

namespace SellnBuy.Api.Entities
{
	public record AdvertisementDto
	(
		Guid Id,
		string Title,
		string Description,
		Condition Condition,
		decimal Price,
		DateTimeOffset CreatedDate,
		Guid userId,
		Guid categoryId
	);
	
	public record CreateAdvertisementDto
	(
		[Required] string Title,
		string Description,
		[Required] Condition Condition,
		[Required] decimal Price,
		[Required] Guid userId,
		[Required] Guid categoryId
	);
	
	public record UpdateAdvertisementDto
	(
		[Required] string Title,
		string Description,
		[Required] Condition Condition,
		[Required] decimal Price,
		[Required] Guid userId,
		[Required] Guid categoryId
	);
}