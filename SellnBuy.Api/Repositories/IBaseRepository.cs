namespace SellnBuy.Api.Repositories;

public interface IBaseRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(int id);
    Task CreateAsync(T item);
    Task DeleteAsync(int id);
    Task UpdateAsync(T item);
}