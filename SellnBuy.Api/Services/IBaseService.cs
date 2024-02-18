using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Services;

public interface IBaseService<T, TDto, CreateTDto, UpdateTDto>
{
	Task<IEnumerable<TDto>> GetAllAsync(string? searchTerm = null);
	Task<TDto> GetAsync(int id);
	Task<(TDto, int)> CreateAsync(CreateTDto createItemDto);
	Task DeleteAsync(int id);
	Task UpdateAsync(int id, UpdateTDto updateItemDto);
}