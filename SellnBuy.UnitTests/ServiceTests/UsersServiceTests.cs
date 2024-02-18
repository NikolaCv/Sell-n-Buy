using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SellnBuy.Api;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Entities.Mapper;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Repositories;
using SellnBuy.Api.Services;
using Xunit;
using AutoMapper;

namespace SellnBuy.UnitTests.ServiceTests;

public class UsersServiceTests
{
	private readonly IMapper mapper;
	private readonly Mock<IUsersRepository> repositoryStub = new();

	private static User CreateRandomUser(string? name = null) => new()
	{
		Id = Guid.NewGuid().ToString(),
		Name = name ?? Guid.NewGuid().ToString(),
		Bio = Guid.NewGuid().ToString(),
		PhoneNumber = Guid.NewGuid().ToString(),
		Email = Guid.NewGuid().ToString(),
		CreatedDate = DateTimeOffset.UtcNow
	};

	public UsersServiceTests()
	{
		var config = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile<UserMappingProfile>();
		});

		mapper = config.CreateMapper();
	}


	[Fact]
	public async Task GetAsync_WithUnexistingUser_ThrowsNotFound()
	{
		// Arrange
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<string>()))
				.ReturnsAsync((User)null!);

		var service = new UsersService(repositoryStub.Object, mapper);

		// Act
		// Delay function execution till assert section with act
		Func<Task> act = async () => await service.GetAsync(It.IsAny<string>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task GetAsync_WithExistingUser_ReturnsExpectedUser()
	{
		// Arange
		var expectedUser = CreateRandomUser();
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<string>()))
							.ReturnsAsync(expectedUser);

		var service = new UsersService(repositoryStub.Object, mapper);

		// Act
		var result = await service.GetAsync(It.IsAny<string>());

		// Assert
		result.Should().BeOfType<UserDto>();
		result.Should().BeEquivalentTo(expectedUser, options => options.ExcludingMissingMembers());
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

		var service = new UsersService(repositoryStub.Object, mapper);

		// Act
		var result = await service.GetAllAsync();

		// Assert
		result.Should().BeEquivalentTo(allUsers, options => options.ExcludingMissingMembers());
	}


	[Fact]
	public async Task GetAllAsync_WithMatchingName_ReturnsMatchingUsers()
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

		var service = new UsersService(repositoryStub.Object, mapper);

		// Act
		var result = await service.GetAllAsync(nameToMatch);

		// Assert
		result.Should().OnlyContain(user =>
		user.Name == allUsers[1].Name || user.Name == allUsers[2].Name);
	}


	// [Fact]
	// public async Task CreateAsync_WithUserToCreate_ReturnsCreatedUserDtoAndCreatedUserId()
	// {
	// 	// Arange
	// 	var createdUser = CreateRandomUser();

	// 	repositoryStub.Setup(repo => repo.CreateAsync(It.IsAny<User>()))
	// 						.Callback<User>(user =>
	// 						{
	// 							user.Id = Guid.NewGuid().ToString();
	// 						})
	// 						.Returns(Task.CompletedTask);

	// 	var service = new UsersService(repositoryStub.Object, mapper);

	// 	var createUserDto = mapper.Map<CreateUserDto>(createdUser);

	// 	// Act
	// 	var result = await service.CreateAsync(createUserDto);

	// 	// Assert
	// 	result.Item1.Should().BeEquivalentTo(createUserDto);
	// 	result.Item1.Id.Should().NotBeNullOrEmpty();
	// 	result.Item1.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, new TimeSpan(0, 0, 0, 1));
	// 	result.Item2.Should().NotBeNullOrEmpty();
	// }


	[Fact]
	public async Task UpdateAsync_WithUnexistingUser_ThrowsNotFound()
	{
		// Arange
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<string>()))
						.ReturnsAsync((User)null!);
		repositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<User>()))
						.Returns(Task.CompletedTask);

		var service = new UsersService(repositoryStub.Object, mapper);

		// Act
		// Delay function execution till assert section with act
		Func<Task> act = async () => await service.UpdateAsync(It.IsAny<string>(), It.IsAny<UpdateUserDto>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task UpdateAsync_WithExistingUser_DoesNotThrowException()
	{
		// Arange
		var existingUser = CreateRandomUser();
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<string>()))
						.ReturnsAsync(existingUser);
		repositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<User>()))
						.Returns(Task.CompletedTask);

		var service = new UsersService(repositoryStub.Object, mapper);

		// Act			
		await service.UpdateAsync(It.IsAny<string>(), mapper.Map<UpdateUserDto>(existingUser));

		// Assert
		// service.UpdateAsync() returns void, test will fail if it throws 
		// If it doesn't throw there is no need to check
	}


	[Fact]
	public async Task DeleteAsync_WithUnexistingUser_ThrowsNotFound()
	{
		// Arange
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<string>()))
						.ReturnsAsync((User)null!);

		var service = new UsersService(repositoryStub.Object, mapper);

		// Act		
		// Delay function execution till assert section with act
		Func<Task> act = async () => await service.DeleteAsync(It.IsAny<string>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task DeleteAsync_WithExistingUser_DoesNotThrowException()
	{
		// Arange
		var existingUser = CreateRandomUser();
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<string>()))
						.ReturnsAsync(existingUser);
		repositoryStub.Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
						.Returns(Task.CompletedTask);

		var service = new UsersService(repositoryStub.Object, mapper);

		// Act			
		await service.DeleteAsync(It.IsAny<string>());

		// Assert
		// service.DeleteAsync() returns void, test will fail if it throws 
		// If it doesn't throw there is no need to check
	}
}