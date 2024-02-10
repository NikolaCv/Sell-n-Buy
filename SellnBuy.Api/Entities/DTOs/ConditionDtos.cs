using System.ComponentModel.DataAnnotations;

namespace SellnBuy.Api.Entities.DTOs;

public record ConditionDto(
    int Id,
    string Name,
    string? Description
);

public record CreateConditionDto(
    [Required][StringLength(50)] string Name,
    string? Description
);

public record UpdateConditionDto(
    [Required][StringLength(50)] string Name,
    string? Description
);
