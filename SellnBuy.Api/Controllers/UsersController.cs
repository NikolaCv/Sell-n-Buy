using Microsoft.AspNetCore.Mvc;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Services;

namespace SellnBuy.Api.Controllers
{	
	[ApiController]
	[Route("[controller]")]
	public class UsersController : BaseController<User, UserDto, CreateUserDto, UpdateUserDto>
	{
		public UsersController(IService<User, UserDto, CreateUserDto, UpdateUserDto> service) : base(service)
		{
		}
	}
}