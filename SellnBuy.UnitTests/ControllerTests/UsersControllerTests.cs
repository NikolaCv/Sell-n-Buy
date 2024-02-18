using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;
using SellnBuy.Api;
using SellnBuy.Api.Controllers;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Entities.Mapper;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Services;
using Xunit;

namespace SellnBuy.UnitTests.ControllerTests;

public class UsersControllerTests
{
	private readonly IMapper mapper;
	private readonly Mock<IUsersService> serviceStub = new();

	private static User CreateRandomUser(string? name = null)
	{
		return new()
		{
			Id = Guid.NewGuid().ToString(),
			Name = name ?? Guid.NewGuid().ToString(),
			Bio = Guid.NewGuid().ToString(),
			PhoneNumber = Guid.NewGuid().ToString(),
			Email = Guid.NewGuid().ToString(),
			CreatedDate = DateTimeOffset.UtcNow
		};
	}

	public UsersControllerTests()
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
		serviceStub.Setup(service => service.GetAsync(It.IsAny<string>()))
						.ThrowsAsync(new NotFoundException());

		var controller = new UsersController(serviceStub.Object);

		// Act
		Func<Task> act = async () => await controller.GetAsync(It.IsAny<string>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task GetAsync_WithExistingUser_ReturnsExpectedUser()
	{
		// Arrange
		var expectedUserDto = mapper.Map<UserDto>(CreateRandomUser());
		serviceStub.Setup(service => service.GetAsync(It.IsAny<string>()))
						.ReturnsAsync(expectedUserDto);

		var controller = new UsersController(serviceStub.Object);

		// Act
		var result = await controller.GetAsync(It.IsAny<string>());

		// Assert
		result.Should().BeOfType<UserDto>();
		result.Should().BeEquivalentTo(expectedUserDto);
	}


	[Fact]
	public async Task GetAllAsync_WithExistingUsers_ReturnsAllUsers()
	{
		// Arrange
		var allUsersDto = new[]
		{
			mapper.Map<UserDto>(CreateRandomUser()),
			mapper.Map<UserDto>(CreateRandomUser()),
			mapper.Map<UserDto>(CreateRandomUser()),
		};
		serviceStub.Setup(service => service.GetAllAsync(It.IsAny<string>()))
						.ReturnsAsync(allUsersDto);

		var controller = new UsersController(serviceStub.Object);

		// Act
		var result = await controller.GetAllAsync();

		// Assert
		result.Should().BeEquivalentTo(allUsersDto);
	}


	[Fact]
	public async Task GetAllAsync_WithMatchingName_ReturnsMachingUsers()
	{
		// Arrange
		var allUsersDto = new[]
		{
			mapper.Map<UserDto>(CreateRandomUser("Nikola Aleksic")),
			mapper.Map<UserDto>(CreateRandomUser("Aleksa Markovic")),
			mapper.Map<UserDto>(CreateRandomUser("Marko")),
			mapper.Map<UserDto>(CreateRandomUser("Nikola"))
		};
		string nameToMatch = "Marko";

		serviceStub.Setup(service => service.GetAllAsync(It.IsAny<string>()))
							.ReturnsAsync((string nameToMatch) =>
							allUsersDto.Where(user =>
							user.Name.Contains(nameToMatch, StringComparison.OrdinalIgnoreCase)));

		var controller = new UsersController(serviceStub.Object);

		// Act
		var result = await controller.GetAllAsync(nameToMatch);

		// Assert
		result.Should().OnlyContain(user =>
		user.Name == allUsersDto[1].Name || user.Name == allUsersDto[2].Name);
	}


	// [Fact]
	// public async Task CreateAsync_WithUserToCreate_ReturnsCreatedUser()
	// {
	// 	// Arrange
	// 	var createdUser = CreateRandomUser();
	// 	var createdUserDto = mapper.Map<UserDto>(createdUser);

	// 	serviceStub.Setup(service => service.CreateAsync(It.IsAny<CreateUserDto>()))
	// 						.ReturnsAsync((createdUserDto, createdUser.Id));

	// 	var controller = new UsersController(serviceStub.Object);

	// 	// Act
	// 	var result = await controller.CreateAsync(It.IsAny<CreateUserDto>());

	// 	// Assert
	// 	result.Result.Should().BeOfType<CreatedAtActionResult>();
	// 	var createdResult = result.Result as CreatedAtActionResult;
	// 	createdResult?.Value.Should().BeEquivalentTo(createdUserDto);
	// }


	[Fact]
	public async Task UpdateAsync_WithUnexistingUser_ThrowsNotFound()
	{
		// Arrange
		serviceStub.Setup(service => service.UpdateAsync(It.IsAny<string>(), It.IsAny<UpdateUserDto>()))
							.ThrowsAsync(new NotFoundException());

		var controller = new UsersController(serviceStub.Object);

		// Act
		// Delay function execution till assert section with act
		Func<Task> act = async () => await controller.UpdateAsync(It.IsAny<string>(), It.IsAny<UpdateUserDto>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task UpdateAsync_WithExistingUser_ReturnsNoContent()
	{
		// Arrange
		serviceStub.Setup(service => service.UpdateAsync(It.IsAny<string>(), It.IsAny<UpdateUserDto>()))
							.Returns(Task.CompletedTask);

		var controller = new UsersController(serviceStub.Object);

		// Act
		var result = await controller.UpdateAsync(It.IsAny<string>(), It.IsAny<UpdateUserDto>());

		// Assert
		result.Should().BeOfType<NoContentResult>();
	}


	[Fact]
	public async Task DeleteAsync_WithUnexistingUser_ThrowsNotFound()
	{
		// Arrange
		serviceStub.Setup(service => service.DeleteAsync(It.IsAny<string>()))
							.ThrowsAsync(new NotFoundException());

		var controller = new UsersController(serviceStub.Object);

		// Act
		// Delay function execution till assert section with act
		Func<Task> act = async () => await controller.DeleteAsync(It.IsAny<string>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task DeleteAsync_WithExistingUser_ReturnsNoContent()
	{
		// Arrange
		serviceStub.Setup(service => service.DeleteAsync(It.IsAny<string>()))
							.Returns(Task.CompletedTask);

		var controller = new UsersController(serviceStub.Object);

		// Act
		var result = await controller.DeleteAsync(It.IsAny<string>());

		// Assert
		result.Should().BeOfType<NoContentResult>();
	}
}
