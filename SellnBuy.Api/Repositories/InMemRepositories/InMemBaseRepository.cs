using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Repositories.InMemRepositories
{
	public abstract class InMemRepository<T> : IRepository<T> where T : BaseEntity
	{
		private readonly List<T> items;

		protected InMemRepository(List<T> items)
		{
			this.items = items;
		}
		
		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await Task.FromResult(items);
		}

		public async Task<T> GetAsync(Guid id)
		{
			return await Task.FromResult(items.SingleOrDefault(item => item.Id == id));
		}

		public async Task CreateAsync(T item)
		{
			items.Add(item);
			await Task.CompletedTask;
		}

		public async Task UpdateAsync(T item)
		{
			var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
			items[index] = item;
			await Task.CompletedTask;
		}
		
		public async Task DeleteAsync(Guid id)
		{
			var index = items.FindIndex(item => item.Id == id);
			items.RemoveAt(index);
			await Task.CompletedTask;
		}
	}
}