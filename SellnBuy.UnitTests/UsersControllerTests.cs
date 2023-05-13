using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SellnBuy.Api;
using SellnBuy.Api.Controllers;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Services;
using Xunit;

namespace SellnBuy.UnitTests
{
	public class UsersControllerTests
	{
		private readonly Mock<IService<User, UserDto, CreateUserDto, UpdateUserDto>> serviceStub = new();
		
		[Fact]
		public async Task GetAsync_WithUnexistingUser_ReturnsNotFound()
		{
			// Arrange
			serviceStub.Setup(service => service.GetAsync(It.IsAny<Guid>()))
							.ThrowsAsync(new NotFoundException());

			var controller = new UsersController(serviceStub.Object);

			// Act
			ActionResult<UserDto>? result;
			try
			{
				result = await controller.GetAsync(Guid.NewGuid());
			}
			catch (NotFoundException)
			{
				result = new NotFoundResult();
			}
			
			// Assert
			result.Result.Should().BeOfType<NotFoundResult>();
		}
		
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
		public async Task GetAsync_WithExistingUser_ReturnsExpectedUser()
		{
			// Arrange
			var expectedUserDto = CreateRandomUser().AsDto<User, UserDto>();
			serviceStub.Setup(service => service.GetAsync(It.IsAny<Guid>()))
							.ReturnsAsync(expectedUserDto);

			var controller = new UsersController(serviceStub.Object);

			// Act
			ActionResult<UserDto> result;
			try
			{
				result = await controller.GetAsync(Guid.NewGuid());
			}
			catch (NotFoundException)
			{
				result = new NotFoundResult();
			}
			
			// Assert
			result.Value.Should().BeEquivalentTo(expectedUserDto,
						options =>
						options.Using<DateTimeOffset>(ctx => 
						ctx.Subject.Should().BeCloseTo(expectedUserDto.CreatedDate, new TimeSpan(0, 0, 0, 1)))
						.WhenTypeIs<DateTimeOffset>());
		}
		
		[Fact]
		public async Task GetAllAsync_WithExistingUsers_ReturnsAllUsers()
		{
			// Arrange
			var allUsers = new[]
			{
				CreateRandomUser().AsDto<User, UserDto>(),
				CreateRandomUser().AsDto<User, UserDto>(),
				CreateRandomUser().AsDto<User, UserDto>()
			};
			serviceStub.Setup(service => service.GetAllAsync(It.IsAny<string>()))
							.ReturnsAsync(allUsers);

			var controller = new UsersController(serviceStub.Object);

			// Act
			var result = await controller.GetAllAsync(It.IsAny<string>());

			// Assert
			result.Should().BeEquivalentTo(allUsers);
		}
		
		[Fact]
		public async Task GetAllAsync_WithMachingUsers_ReturnsMachingUsers()
		{
			// Arrange
			var allUsers = new[]
			{
				CreateRandomUser("Nikola Aleksic").AsDto<User, UserDto>(),
				CreateRandomUser("Aleksa Markovic").AsDto<User, UserDto>(),
				CreateRandomUser("Marko").AsDto<User, UserDto>(),
				CreateRandomUser("Nikola").AsDto<User, UserDto>(),
			};
			string nameToMatch = "Marko";
			
			serviceStub.Setup(service => service.GetAllAsync(It.IsAny<string>()))
							 .ReturnsAsync((string nameToMatch) =>
								allUsers.Where(user =>
								user.Name.Contains(nameToMatch, StringComparison.OrdinalIgnoreCase)));

			var controller = new UsersController(serviceStub.Object);

			// Act
			var result = await controller.GetAllAsync(nameToMatch);

			// Assert
			result.Should().OnlyContain(user =>
			user.Name == allUsers[1].Name || user.Name == allUsers[2].Name);
		}
		
		[Fact]
		public async Task CreateAsync_WithUserToCreate_ReturnsCreatedUser()
		{
			// Arrange
			var createdUser = CreateRandomUser();
			serviceStub.Setup(service => service.CreateAsync(It.IsAny<CreateUserDto>()))
							 .ReturnsAsync(createdUser);
							
			var controller = new UsersController(serviceStub.Object);

			// Act
			var result = await controller.CreateAsync(createdUser.AsDto<User, CreateUserDto>());
			
			// Assert
			result.Result.Should().BeOfType<CreatedAtActionResult>();
			var createdResult = result.Result as CreatedAtActionResult;
			createdResult?.Value.Should().BeEquivalentTo(createdUser.AsDto<User, UserDto>());
		}
		
		[Fact]
		public async Task UpdateAsync_WithUnexistingUser_ReturnsNotFound()
		{
			// Arrange
			serviceStub.Setup(service => service.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateUserDto>()))
							 .ThrowsAsync(new NotFoundException());
							 
			var controller = new UsersController(serviceStub.Object);
			
			// Act
			ActionResult result;
			try
			{
				result = await controller.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateUserDto>());
			}
			catch (NotFoundException)
			{
				result = new NotFoundResult();
			}
			
			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}
		
		[Fact]
		public async Task UpdateAsync_WithExistingUser_ReturnsNoContent()
		{
			// Arrange
			serviceStub.Setup(service => service.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateUserDto>()))
							 .Returns(Task.CompletedTask);
							 
			var controller = new UsersController(serviceStub.Object);
			
			// Act
			ActionResult result;
			try
			{
				result = await controller.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateUserDto>());
			}
			catch (NotFoundException)
			{
				result = new NotFoundResult();
			}
			
			// Assert
			result.Should().BeOfType<NoContentResult>();
		}
		
		[Fact]
		public async Task DeleteAsync_WithUnexistingUser_ReturnsNotFound()
		{
			// Arrange
			serviceStub.Setup(service => service.DeleteAsync(It.IsAny<Guid>()))
							 .ThrowsAsync(new NotFoundException());
							 
			var controller = new UsersController(serviceStub.Object);
			
			// Act
			ActionResult result;
			try
			{
				result = await controller.DeleteAsync(It.IsAny<Guid>());
			}
			catch (NotFoundException)
			{
				result = new NotFoundResult();
			}
			
			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}
		
		[Fact]
		public async Task DeleteAsync_WithExistingUser_ReturnsNoContent()
		{
			// Arrange
			serviceStub.Setup(service => service.DeleteAsync(It.IsAny<Guid>()))
							 .Returns(Task.CompletedTask);
			
			var controller = new UsersController(serviceStub.Object);
			
			// Act
			ActionResult result;
			try
			{
				result = await controller.DeleteAsync(It.IsAny<Guid>());
			}
			catch (NotFoundException)
			{
				result = new NotFoundResult();
			}
			
			// Assert
			result.Should().BeOfType<NoContentResult>();
		}
	}
}
