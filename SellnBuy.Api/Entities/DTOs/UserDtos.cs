using System.ComponentModel.DataAnnotations;

namespace SellnBuy.Api.Entities.DTOs;

public record UserDto(
	int Id,
	string Name,
	string? Bio,
	string? PhoneNumber,
	string Email,
	DateTimeOffset CreatedDate
);

public record CreateUserDto(
	[Required][StringLength(50)] string Name,
	string? Bio,
	string? PhoneNumber,
	[Required][StringLength(50)] string Email
);

public record UpdateUserDto(
	[Required][StringLength(50)] string Name,
	string? Bio,
	string? PhoneNumber,
	[Required][StringLength(50)] string Email
);
