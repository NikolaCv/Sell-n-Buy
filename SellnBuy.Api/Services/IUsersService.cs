using SellnBuy.Api.Entities.DTOs;

namespace SellnBuy.Api.Services;

public interface IUsersService
{
    Task<IEnumerable<UserDto>> GetAllAsync(string? name = null);
    Task<UserDto> GetAsync(string id);
    // Task<(UserDto, string)> CreateAsync(CreateUserDto createUserDto);
    Task DeleteAsync(string id);
    Task UpdateAsync(string id, UpdateUserDto updateUserDto);
}