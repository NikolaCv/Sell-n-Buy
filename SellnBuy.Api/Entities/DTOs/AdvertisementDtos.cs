using System.ComponentModel.DataAnnotations;

namespace SellnBuy.Api.Entities.DTOs;

public record AdvertisementDto(
	int Id,
	string Title,
	string? Description,
	decimal Price,
	DateTimeOffset CreatedDate,
	int ConditionId,
	Condition Condition,
	int UserId,
	User User,
	int CategoryId,
	Category Category
);

public record CreateAdvertisementDto(
	[StringLength(50)] string Title,
	string? Description,
	[Range(1, 10e8)] decimal Price,
	int ConditionId,
	int UserId,
	int CategoryId
);

public record UpdateAdvertisementDto(
	[StringLength(50)] string Title,
	string? Description,
	[Range(1, 10e8)] decimal Price,
	int ConditionId,
	int UserId,
	int CategoryId
);
