using System.ComponentModel.DataAnnotations;

namespace SellnBuy.Api.Entities.DTOs;

public record CategoryDto(
	int Id,
	string Name,
	string? Description,
	DateTimeOffset CreatedDate
);

public record CreateCategoryDto(
	[StringLength(50)] string Name,
	string? Description
);

public record UpdateCategoryDto(
	[StringLength(50)] string Name,
	string? Description
);
