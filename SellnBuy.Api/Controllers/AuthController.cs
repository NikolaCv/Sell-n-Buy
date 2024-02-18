using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Services;

namespace SellnBuy.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService authService;

	public AuthController(IAuthService authService)
	{
		this.authService = authService;
	}

	[HttpPost("login")]
	public async Task<ActionResult<string>> Login(LoginUserDto model)
	{
		var token = await authService.AuthenticateAsync(model);
		return Ok(token);
	}

	[HttpPost("register")]
	public async Task<ActionResult<UserDto>> Register(RegisterUserDto model)
	{

		var registeredUserDto = await authService.RegisterAsync(model);
		return CreatedAtAction(nameof(Register), new { id = registeredUserDto.Id }, registeredUserDto);
	}
}
