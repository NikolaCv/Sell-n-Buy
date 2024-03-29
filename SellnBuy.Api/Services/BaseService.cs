using AutoMapper;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Repositories;

namespace SellnBuy.Api.Services;

public abstract class BaseService<T, TDto, CreateTDto, UpdateTDto> : IBaseService<T, TDto, CreateTDto, UpdateTDto>
where T : BaseEntity
{
	protected readonly IMapper mapper;

	protected readonly IBaseRepository<T> repository;

	public BaseService(IBaseRepository<T> repository, IMapper mapper)
	{
		this.mapper = mapper;
		this.repository = repository;
	}

	public abstract Task<IEnumerable<TDto>> GetAllAsync(string? searchTerm = null);
	public async Task<TDto> GetAsync(int id)
	{
		var item = await repository.GetAsync(id) ?? throw new NotFoundException(typeof(T));
		return mapper.Map<TDto>(item);
	}

	public async Task<(TDto, int)> CreateAsync(CreateTDto createItemDto)
	{
		var newItem = mapper.Map<T>(createItemDto);
		newItem.CreatedDate = DateTimeOffset.UtcNow;
		await repository.CreateAsync(newItem);

		return (mapper.Map<TDto>(newItem), newItem.Id);
	}

	public async Task UpdateAsync(int id, UpdateTDto updateItemDto)
	{
		var existingItem = await repository.GetAsync(id) ?? throw new NotFoundException(typeof(T));

		mapper.Map(updateItemDto, existingItem);
		await repository.UpdateAsync(existingItem);
	}

	public async Task DeleteAsync(int id)
	{
		_ = await repository.GetAsync(id) ?? throw new NotFoundException(typeof(T));
		await repository.DeleteAsync(id);
	}
}