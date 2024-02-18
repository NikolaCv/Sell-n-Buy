using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;

namespace SellnBuy.Api.Services;

public class AuthService : IAuthService
{
	private readonly UserManager<User> userManager;
	private readonly RoleManager<IdentityRole> roleManager;
	private readonly IMapper mapper;
	private readonly string jwtSecret;


	public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration configuration)
	{
		this.userManager = userManager;
		this.roleManager = roleManager;
		this.mapper = mapper;
		jwtSecret = configuration["JwtSettings:Secret"] ?? throw new ArgumentNullException(nameof(configuration));
	}


	public async Task<string> AuthenticateAsync(LoginUserDto model)
	{
		var user = await userManager.FindByEmailAsync(model.Email);

		if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
			throw new ApplicationException("Invalid username or password.");

		return GenerateJwtToken(user, model.RememberMe);
	}

	public async Task<UserDto> RegisterAsync(RegisterUserDto model)
	{
		var user = mapper.Map<User>(model);
		user.UserName = user.Email; //  FIXME

		var result = await userManager.CreateAsync(user, model.Password);
		if (!result.Succeeded) throw new ApplicationException("Failed to create user."); // TODO password validation da kaze kad je fail

		result = await userManager.AddToRoleAsync(user, "User");
		if (!result.Succeeded) throw new ApplicationException("Failed to add a role.");

		return mapper.Map<UserDto>(user);
	}

	public string GenerateJwtToken(User user, bool rememberMe)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(jwtSecret);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new(ClaimTypes.NameIdentifier, user.Id),
			}),
			Expires = rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(1),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
														SecurityAlgorithms.HmacSha256Signature)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}
}