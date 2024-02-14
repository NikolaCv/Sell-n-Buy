using System.ComponentModel.DataAnnotations;
using SellnBuy.Api.Entities.Validation;

namespace SellnBuy.Api.Entities.DTOs;

public record AdvertisementDto(
	int Id,
	string Title,
	string? Description,
	decimal Price,
	DateTimeOffset CreatedDate,
	int ConditionId,
	int UserId,
	int CategoryId,
	User User,
	Condition Condition,
	Category Category
);

public record CreateAdvertisementDto(
	[StringLength(50)] string Title,
	string? Description,
	[Range(1, 10e8)] decimal Price,
	[IdRequired] int ConditionId,
	[IdRequired] int UserId,
	[IdRequired] int CategoryId
);

public record UpdateAdvertisementDto(
	[StringLength(50)] string Title,
	string? Description,
	[Range(1, 10e8)] decimal Price,
	[IdRequired] int ConditionId,
	[IdRequired] int UserId,
	[IdRequired] int CategoryId
);
