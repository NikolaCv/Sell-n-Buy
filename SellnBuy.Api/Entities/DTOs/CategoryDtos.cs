using System.ComponentModel.DataAnnotations;

namespace SellnBuy.Api.Entities.DTOs;

public record CategoryDto(
	int Id,
	string Name,
	string? Description
);

public record CreateCategoryDto(
	[Required][StringLength(50)] string Name,
	string? Description
);

public record UpdateCategoryDto(
	[Required][StringLength(50)] string Name,
	string? Description
);
