using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(int id);
    Task CreateAsync(T item);
    Task DeleteAsync(int id);
    Task UpdateAsync(T item);
}