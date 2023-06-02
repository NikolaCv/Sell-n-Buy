using FluentAssertions;
using Moq;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Repositories;
using SellnBuy.Api.Services;
using Xunit;

namespace SellnBuy.UnitTests.ServiceTests
{
	public class UsersServiceTests
	{
		private readonly Mock<IRepository<User>> repositoryStub = new();
		
		private User CreateRandomUser(string? name = null)
		{
			return new()
			{
				Id = Guid.NewGuid(),
				Name = name ?? Guid.NewGuid().ToString(),
				Bio = Guid.NewGuid().ToString(),
				PhoneNumber = Guid.NewGuid().ToString(),
				Email = Guid.NewGuid().ToString(),
				CreatedDate = DateTimeOffset.UtcNow
			};
		}
		
		
		[Fact]
		public async Task GetAsync_WithUnexistingUser_ThrowsNotFound()
		{
			// Arrange
			repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
				 .ReturnsAsync((User?)null!);
			
			var service = new UsersService(repositoryStub.Object);

			// Act
			Func<Task> act = async () => await service.GetAsync(It.IsAny<Guid>());
			
			// Assert
			await act.Should().ThrowAsync<NotFoundException>();
		}
		
		
		[Fact]
		public async Task GetAsync_WithExistingUser_ReturnsExpectedUser()
		{
			// Arange
			var expectedUser = CreateRandomUser();
			repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
								.ReturnsAsync(expectedUser);
						
			var service = new UsersService(repositoryStub.Object);
			
			// Act
			var result = await service.GetAsync(It.IsAny<Guid>());
			
			// Assert
			result.Should().BeEquivalentTo(expectedUser,
						options =>
						options.Using<DateTimeOffset>(ctx => 
						ctx.Subject.Should().BeCloseTo(expectedUser.CreatedDate, new TimeSpan(0, 0, 0, 1)))
						.WhenTypeIs<DateTimeOffset>());
		}
		
		
		[Fact]
		public async Task GetAllAsync_WithExistingUsers_ReturnsAllUsers()
		{
			// Arange
			var allUsers = new[]
			{
				CreateRandomUser(),
				CreateRandomUser(),
				CreateRandomUser()
			};
			repositoryStub.Setup(repo => repo.GetAllAsync())
								.ReturnsAsync(allUsers);
								
			var service = new UsersService(repositoryStub.Object);
			
			// Act
			var result = await service.GetAllAsync();
			
			// Assert
			result.Should().BeEquivalentTo(allUsers);
		}
		
		
		[Fact]
		public async Task GetAllAsync_WithMatchingUsers_ReturnsMatchingUsers()
		{
			// Arange
			var allUsers = new[]
			{
				CreateRandomUser("Nikola Aleksic"),
				CreateRandomUser("Aleksa Markovic"),
				CreateRandomUser("Marko"),
				CreateRandomUser("Nikola")
			};
			string nameToMatch = "Marko";
			
			repositoryStub.Setup(repo => repo.GetAllAsync())
								.ReturnsAsync(allUsers);
								
			var service = new UsersService(repositoryStub.Object);
			
			// Act
			var result = await service.GetAllAsync(nameToMatch);
			
			// Assert
			result.Should().OnlyContain(user =>
			user.Name == allUsers[1].Name || user.Name == allUsers[2].Name);
		}
		
		// Arange
		
		// Act
		
		// Assert
		
	}
}