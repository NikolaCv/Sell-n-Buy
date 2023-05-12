using SellnBuy.Api.Entities;
using MongoDB.Driver;

namespace SellnBuy.Api.Repositories.MongoDbRepositories
{
	public abstract class MongoDbRepository<T> : IRepository<T> where T : BaseEntity
	{
		private readonly string databaseName;
		private readonly string collectionName;
		private readonly IMongoCollection<T> itemsCollection;
		private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

		protected MongoDbRepository(string databaseName, string collectionName, IMongoCollection<T> itemsCollection)
		{
			this.databaseName = databaseName;
			this.collectionName = collectionName;
			this.itemsCollection = itemsCollection;
		}

		public Task CreateAsync(T item)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<T>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<T> GetAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(T item)
		{
			throw new NotImplementedException();
		}
	}
}