using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Services;

public interface IService<T, TDto, CreateTDto, UpdateTDto>
where T : BaseEntity
{
	Task<IEnumerable<TDto>> GetAllAsync(string? searchTerm = null);
	Task<TDto> GetAsync(int id);
	Task<(TDto, int)> CreateAsync(CreateTDto item);
	Task DeleteAsync(int id);
	Task UpdateAsync(int id, UpdateTDto itemDto);
}