using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Repositories.InMemRepositories
{
	public class InMemUsersRepository : InMemRepository<User>
	{
		public InMemUsersRepository() : base(new List<User>
		{
			new User
			{
				Id = Guid.NewGuid(),
				Name = "Aleksa Starcevic",
				Bio = "asd one",
				PhoneNumber = "+38125465465",
				Email = "aleksa@gmail.com",
				CreatedDate = DateTimeOffset.UtcNow 
			},
			new User
			{
				Id = Guid.NewGuid(),
				Name = "Nikola Cvetanovic", 
				Bio = "asd one", 
				PhoneNumber = "+38112312312", 
				Email = "nikola@gmail.com", 
				CreatedDate = DateTimeOffset.UtcNow
			},
			new User
			{ 
				Id = Guid.NewGuid(),
				Name = "Aleksa Visnjevac", 
				Bio = "asd one", 
				PhoneNumber = "+38145368455", 
				Email = "visnja@gmail.com", 
				CreatedDate = DateTimeOffset.UtcNow
			}
		})
		{
		}
	}
}
		
		
