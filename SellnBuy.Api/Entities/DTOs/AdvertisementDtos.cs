using System.ComponentModel.DataAnnotations;
using SellnBuy.Api.Entities.Validation;

namespace SellnBuy.Api.Entities.DTOs;

public record AdvertisementDto(
	int Id,
	string Title,
	string? Description,
	decimal Price,
	DateTimeOffset CreatedDate,
	UserDto User, // When renaming to EntityDto modify AdvertisementMappingProfile
	ConditionDto Condition,
	CategoryDto Category
);

public record CreateAdvertisementDto(
	[StringLength(50)] string Title,
	string? Description,
	[Range(1, 10e8)] decimal Price,
	[IntIdRequired] int ConditionId,
	[StringIdRequired] string UserId,
	[IntIdRequired] int CategoryId
);

public record UpdateAdvertisementDto(
	[StringLength(50)] string Title,
	string? Description,
	[Range(1, 10e8)] decimal Price,
	[IntIdRequired] int ConditionId,
	[StringIdRequired] string UserId,
	[IntIdRequired] int CategoryId
);
