using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Repositories;

public interface IUsersRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetAsync(string id);
    // Task CreateAsync(User user);
    Task DeleteAsync(string id);
    Task UpdateAsync(User user);
}