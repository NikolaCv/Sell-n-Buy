using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;

namespace SellnBuy.Api.Services;

public interface IAuthService
{
	Task<string> AuthenticateAsync(LoginUserDto model);
	string GenerateJwtToken(User user, bool rememberMe);
	Task<UserDto> RegisterAsync(RegisterUserDto model);

}