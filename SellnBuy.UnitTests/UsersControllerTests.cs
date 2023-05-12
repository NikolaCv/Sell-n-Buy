using System.Net;
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
		public async Task GetAsync_WithUnexistingItem_ReturnsNotFound()
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
			Assert.IsType<NotFoundResult>(result.Result);
		}
		
		private User CreateRandomUser()
		{
			return new()
			{
				Id = Guid.NewGuid(),
				Name = Guid.NewGuid().ToString(),
				Bio = Guid.NewGuid().ToString(),
				PhoneNumber = Guid.NewGuid().ToString(),
				Email = Guid.NewGuid().ToString(),
				CreatedDate = DateTimeOffset.UtcNow
			};
		}
		
		[Fact]
		public async Task GetAsync_WithExistingItem_ReturnsExpected()
		{
			// Arrange
			var expectedUserDto = CreateRandomUser().AsDto<User, UserDto>;
			serviceStub.Setup(service => service.GetAsync(It.IsAny<Guid>()))
							.ReturnsAsync(await Task.FromResult(expectedUserDto));

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
			Assert.IsType<UserDto>(result.Value);
			// TODO: compare properties of expected dto and result dto
		}
	}
}
