using AutoMapper;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Repositories;

namespace SellnBuy.Api.Services;

public class UsersService : IUsersService
{
	private readonly IMapper mapper;

	private readonly IUsersRepository repository;

	public UsersService(IUsersRepository repository, IMapper mapper)
	{
		this.mapper = mapper;
		this.repository = repository;
	}

	public async Task<IEnumerable<UserDto>> GetAllAsync(string? name = null)
	{
		var users = await repository.GetAllAsync();

		if (!string.IsNullOrWhiteSpace(name))
			users = users.Where(item => item.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

		return users.Select(mapper.Map<UserDto>);
	}

	public async Task<UserDto> GetAsync(string id)
	{
		var user = await repository.GetAsync(id) ?? throw new NotFoundException(typeof(User));
		return mapper.Map<UserDto>(user);
	}

	// public async Task<(UserDto, string)> CreateAsync(CreateUserDto createUserDto)
	// {
	// 	var newUser = mapper.Map<User>(createUserDto);
	// 	newUser.CreatedDate = DateTimeOffset.UtcNow;
	// 	await repository.CreateAsync(newUser);

	// 	return (mapper.Map<UserDto>(newUser), newUser.Id);
	// }

	public async Task UpdateAsync(string id, UpdateUserDto updateUserDto)
	{
		var existingUser = await repository.GetAsync(id) ?? throw new NotFoundException(typeof(User));

		mapper.Map(updateUserDto, existingUser);
		await repository.UpdateAsync(existingUser);
	}

	public async Task DeleteAsync(string id)
	{
		_ = await repository.GetAsync(id) ?? throw new NotFoundException(typeof(User));
		await repository.DeleteAsync(id);
	}
}