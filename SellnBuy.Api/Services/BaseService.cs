using SellnBuy.Api.Entities;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Repositories;

namespace SellnBuy.Api.Services
{
	public abstract class BaseService<T, TDto, CreateTDto, UpdateTDto> : IService<T, TDto, CreateTDto, UpdateTDto>
	where T : BaseEntity
	where TDto : class
	where CreateTDto : class
	where UpdateTDto : class
	{
		protected readonly IRepository<T> repository;
		
		public BaseService(IRepository<T> repository)
		{
			this.repository = repository;
		}

		public abstract Task<IEnumerable<TDto>> GetAllAsync(string? searchTerm);

		public async Task<TDto> GetAsync(Guid id)
		{
			var item = await repository.GetAsync(id);
			
			if (item is null) throw new NotFoundException();
			
			return item.AsDto<T, TDto>();
		}
		
		public async Task<T> CreateAsync(CreateTDto itemDto)
		{
			var newItem = itemDto.AsEntity<T, CreateTDto>(Guid.NewGuid(), DateTimeOffset.UtcNow);
			
			await repository.CreateAsync(newItem);
			
			return newItem;
		}

		public async Task UpdateAsync(Guid id, UpdateTDto itemDto)
		{
			var existingItem = await repository.GetAsync(id);
			
			if (existingItem is null) throw new NotFoundException();
			
			existingItem = itemDto.AsEntity<T, UpdateTDto>(existingItem.Id, existingItem.CreatedDate);
			
			await repository.UpdateAsync(existingItem);
		}
		
		public async Task DeleteAsync(Guid id)
		{
			var existingItem = await repository.GetAsync(id);
			
			if (existingItem is null) throw new NotFoundException();
					
			await repository.DeleteAsync(id);		
		}


	}
}