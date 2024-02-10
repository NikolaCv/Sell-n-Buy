using System.ComponentModel.DataAnnotations;

namespace SellnBuy.Api.Entities.DTOs;

public record AdvertisementDto(
	int Id,
	string Title,
	string? Description,
	decimal Price,
	DateTimeOffset CreatedDate,
	int ConditionId,
	int UserId,
	int CategoryId
);

public record CreateAdvertisementDto(
	[Required][StringLength(50)] string Title,
	string? Description,
	[Required][Range(0, 10e8)] decimal Price,
	[Required] int ConditionId,
	[Required] int UserId,
	[Required] int CategoryId
);

public record UpdateAdvertisementDto(
	[Required][StringLength(50)] string Title,
	string? Description,
	[Required][Range(0, 10e8)] decimal Price,
	[Required] int ConditionID,
	[Required] int UserId,
	[Required] int CategoryId
);
