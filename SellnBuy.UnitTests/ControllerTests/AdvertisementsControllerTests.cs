
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Moq;
using SellnBuy.Api;
using SellnBuy.Api.Controllers;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Services;
using Xunit;

namespace SellnBuy.UnitTests.ControllerTests
{
	public class AdvertisementsControllerTests
	{
		private readonly Mock<IService<Advertisement, AdvertisementDto, CreateAdvertisementDto, UpdateAdvertisementDto>> serviceStub = new();
		
		private Advertisement CreateRandomAdvertisement(string? title = null)
		{
			return new()
			{
				Id = Guid.NewGuid(),
				Title = title ?? Guid.NewGuid().ToString(),
				Description = Guid.NewGuid().ToString(),
				CreatedDate = DateTimeOffset.UtcNow,
				UserId = Guid.NewGuid(),
				CategoryId = Guid.NewGuid()
			};
		}
		
		
		[Fact]
		public async Task GetAsync_WithUnexistingAdvertisement_ThrowsNotFound()
		{
			// Arrange
			serviceStub.Setup(service => service.GetAsync(It.IsAny<Guid>()))
							 .ThrowsAsync(new NotFoundException());
							 
			var controller = new AdvertisementsController(serviceStub.Object);
			
			// Act
			Func<Task> act = async () => await controller.GetAsync(It.IsAny<Guid>());
			
			// Assert
			await act.Should().ThrowAsync<NotFoundException>();
		}
		
		
		[Fact]
		public async Task GetAsync_WithExistingAdvertisement_ReturnsExpectedAdvertisement()
		{
			// Arrange
			var advertisementDto = CreateRandomAdvertisement().AsDto<Advertisement, AdvertisementDto>();
			serviceStub.Setup(service => service.GetAsync(It.IsAny<Guid>()))
							 .ReturnsAsync(advertisementDto);
							 
			var controller = new AdvertisementsController(serviceStub.Object);
			
			// Act
			var	result = await controller.GetAsync(It.IsAny<Guid>());
			
			// Assert
			result.Should().BeEquivalentTo(advertisementDto);
		}
		
		
		[Fact]
		public async Task GetAllAsync_WithExistingAdvertisements_ReturnsAllAdvertisements()
		{
			// Arrange
			var allAdvertisements = new[]
			{
				CreateRandomAdvertisement().AsDto<Advertisement, AdvertisementDto>(),
				CreateRandomAdvertisement().AsDto<Advertisement, AdvertisementDto>(),
				CreateRandomAdvertisement().AsDto<Advertisement, AdvertisementDto>()
			};
			
			serviceStub.Setup(service => service.GetAllAsync(It.IsAny<string>()))
							 .ReturnsAsync(allAdvertisements);
							 
			var controller = new AdvertisementsController(serviceStub.Object);

			// Act
			var result = await controller.GetAllAsync();
			
			// Assert
			result.Should().BeEquivalentTo(allAdvertisements);
		}
		
		
		[Fact]
		public async Task GetAllAsync_WithMachingAdvertisements_ReturnsMachingAdvertisements()
		{
			// Arrange
			var allAdvertisements = new[]
			{
				CreateRandomAdvertisement("Trabant ko nov").AsDto<Advertisement, AdvertisementDto>(),
				CreateRandomAdvertisement("Reno Clio").AsDto<Advertisement, AdvertisementDto>(),
				CreateRandomAdvertisement("Open Astra").AsDto<Advertisement, AdvertisementDto>(),
				CreateRandomAdvertisement("Reno Senic").AsDto<Advertisement, AdvertisementDto>(),
				CreateRandomAdvertisement("Mitsubishi").AsDto<Advertisement, AdvertisementDto>()
			};
			string titleToMatch = "Reno";
			
			serviceStub.Setup(service => service.GetAllAsync(It.IsAny<string>()))
							 .ReturnsAsync((string titleToMatch) =>
								allAdvertisements.Where(ad =>
								ad.Title.Contains(titleToMatch, StringComparison.OrdinalIgnoreCase)));
								
			var controller = new AdvertisementsController(serviceStub.Object);
			
			// Act
			var result = await controller.GetAllAsync(titleToMatch);
			
			// Assert
			result.Should().OnlyContain(ad =>
			ad.Title == allAdvertisements[1].Title || ad.Title == allAdvertisements[3].Title);
		}
		
		
		[Fact]
		public async Task CreateAsync_WithAdvertisementToCreate_ReturnsCreatedAdvertisement()
		{
			// Arrange
			var createdAdvertisement = CreateRandomAdvertisement();
			var createdAdvertisementDto = createdAdvertisement.AsDto<Advertisement, AdvertisementDto>();
			serviceStub.Setup(service => service.CreateAsync(It.IsAny<CreateAdvertisementDto>()))
							 .ReturnsAsync((createdAdvertisementDto, createdAdvertisement.Id));
							 
			var controller = new AdvertisementsController(serviceStub.Object);
			
			// Act
			var result = await controller.CreateAsync(It.IsAny<CreateAdvertisementDto>());

			// Assert
			result.Result.Should().BeOfType<CreatedAtActionResult>();
			var createdResult = result.Result as CreatedAtActionResult;
			createdResult?.Value.Should().BeEquivalentTo(createdAdvertisementDto);
		}
		
		
		[Fact]
		public async Task UpdateAsync_WithUnexistingAdvertisement_ThrowsNotFound()
		{
			// Arrange
			serviceStub.Setup(service => service.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateAdvertisementDto>()))
							 .ThrowsAsync(new NotFoundException());
			
			var controller = new AdvertisementsController(serviceStub.Object);
			
			// Act
			Func<Task> act = async () => await controller.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateAdvertisementDto>());
			
			// Assert
			await act.Should().ThrowAsync<NotFoundException>();
		}
		
		
		[Fact]
		public async Task UpdateAsync_WithExistingAdvertisement_ReturnsNoContent()
		{
			// Arrange
			var updatedAdvertisement = CreateRandomAdvertisement();
			serviceStub.Setup(service => service.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateAdvertisementDto>()))
							 .Returns(Task.CompletedTask);
			
			var controller = new AdvertisementsController(serviceStub.Object);
			
			// Act
			var result = await controller.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateAdvertisementDto>());
						
			// Assert
			result.Should().BeOfType<NoContentResult>();
		}
		
		
		[Fact]
		public async Task DeleteAsync_WithUnexistingAdvertisement_ThrowsNotFound()
		{
			// Arrange
			serviceStub.Setup(service => service.DeleteAsync(It.IsAny<Guid>()))
							 .ThrowsAsync(new NotFoundException());
			
			var controller = new AdvertisementsController(serviceStub.Object);
			
			// Act
			Func<Task> act = async () => await controller.DeleteAsync(It.IsAny<Guid>());
			
			// Assert
			await act.Should().ThrowAsync<NotFoundException>();
		}
		
		
		[Fact]
		public async Task DeleteAsync_WithExistingAdvertisement_ReturnsNoContent()
		{
			// Arrange
			var updatedAdvertisement = CreateRandomAdvertisement();
			serviceStub.Setup(service => service.DeleteAsync(It.IsAny<Guid>()))
							 .Returns(Task.CompletedTask);
			
			var controller = new AdvertisementsController(serviceStub.Object);
			
			// Act
			var result = await controller.DeleteAsync(It.IsAny<Guid>());
						
			// Assert
			result.Should().BeOfType<NoContentResult>();
		}
	}
}