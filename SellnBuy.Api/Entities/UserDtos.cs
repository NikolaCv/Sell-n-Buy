using System.ComponentModel.DataAnnotations;

namespace SellnBuy.Api.Entities
{
	public record UserDto
	(
		Guid Id,
		string Name,
		string Bio,
		string PhoneNumber,
		string Email,
		DateTimeOffset CreatedDate
	);
	
	public record CreateUserDto
	(
		[Required] string Name,
		string Bio,
		string PhoneNumber,
		[Required] string Email
	);
	
	public record UpdateUserDto
	(
		[Required] string Name,
		string Bio,
		string PhoneNumber,
		[Required] string Email
	);
}