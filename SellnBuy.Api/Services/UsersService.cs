using AutoMapper;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Repositories;

namespace SellnBuy.Api.Services;

public class UsersService : BaseService<User, UserDto, CreateUserDto, UpdateUserDto>
{
	public UsersService(IRepository<User> repository, IMapper mapper) : base(repository, mapper)
	{
	}

	public override async Task<IEnumerable<UserDto>> GetAllAsync(string? name = null)
	{
		var users = await repository.GetAllAsync();

		if (!string.IsNullOrWhiteSpace(name))
			users = users.Where(user => user.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

		return users.Select(mapper.Map<UserDto>);
	}
}