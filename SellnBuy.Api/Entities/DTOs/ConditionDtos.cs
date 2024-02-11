using System.ComponentModel.DataAnnotations;

namespace SellnBuy.Api.Entities.DTOs;

public record ConditionDto(
	int Id,
	string Name,
	string? Description,
	DateTimeOffset CreatedDate
);

public record CreateConditionDto(
	[StringLength(50)] string Name,
	string? Description
);

public record UpdateConditionDto(
	[StringLength(50)] string Name,
	string? Description
);
