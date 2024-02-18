using System.ComponentModel.DataAnnotations;

namespace SellnBuy.Api.Entities.DTOs;

public record UserDto(
	string Id,
	string Name,
	string? Bio,
	string? PhoneNumber,
	string Email,
	DateTimeOffset CreatedDate
);

public record CreateUserDto(
	[StringLength(50)] string Name,
	string? Bio,
	string? PhoneNumber,
	[EmailAddress][StringLength(50)] string Email,
	string Password
);

public record RegisterUserDto(
	[StringLength(50)] string Name,
	string? PhoneNumber,
	[EmailAddress][StringLength(50)] string Email,
	[StringLength(50)] string Password
);

public record UpdateUserDto(
	[StringLength(50)] string Name,
	string? Bio,
	string? PhoneNumber,
	[EmailAddress][StringLength(50)] string Email
// [StringLength(50)] string Password
);

public record LoginUserDto(
	[EmailAddress][StringLength(50)] string Email,
	string Password,
	bool RememberMe
);
