
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
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

public class AdvertisementsControllerTests
{
	private readonly IMapper mapper;

	private readonly Mock<IService<Advertisement, AdvertisementDto, CreateAdvertisementDto, UpdateAdvertisementDto>> serviceStub = new();

	private static Advertisement CreateRandomAdvertisement(string? title = null)
	{
		return new()
		{
			Id = new Random().Next(1, 1000),
			Title = title ?? Guid.NewGuid().ToString(),
			Description = Guid.NewGuid().ToString(),
			CreatedDate = DateTimeOffset.UtcNow,
			UserId = new Random().Next(1, 1000),
			CategoryId = new Random().Next(1, 1000)
		};
	}

	public AdvertisementsControllerTests()
	{
		var config = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile<AdvertisementMappingProfile>();
		});

		mapper = config.CreateMapper();
	}


	[Fact]
	public async Task GetAsync_WithUnexistingAdvertisement_ThrowsNotFound()
	{
		// Arrange
		serviceStub.Setup(service => service.GetAsync(It.IsAny<int>()))
							.ThrowsAsync(new NotFoundException());

		var controller = new AdvertisementsController(serviceStub.Object);

		// Act
		Func<Task> act = async () => await controller.GetAsync(It.IsAny<int>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task GetAsync_WithExistingAdvertisement_ReturnsExpectedAdvertisement()
	{
		// Arrange
		var advertisementDto = mapper.Map<AdvertisementDto>(CreateRandomAdvertisement());
		serviceStub.Setup(service => service.GetAsync(It.IsAny<int>()))
							.ReturnsAsync(advertisementDto);

		var controller = new AdvertisementsController(serviceStub.Object);

		// Act
		var result = await controller.GetAsync(It.IsAny<int>());

		// Assert
		result.Should().BeOfType<AdvertisementDto>();
		result.Should().BeEquivalentTo(advertisementDto);
	}


	[Fact]
	public async Task GetAllAsync_WithExistingAdvertisements_ReturnsAllAdvertisements()
	{
		// Arrange
		var allAdvertisementsDto = new[]
		{
			mapper.Map<AdvertisementDto>(CreateRandomAdvertisement()),
			mapper.Map<AdvertisementDto>(CreateRandomAdvertisement()),
			mapper.Map<AdvertisementDto>(CreateRandomAdvertisement())
		};

		serviceStub.Setup(service => service.GetAllAsync(It.IsAny<string>()))
							.ReturnsAsync(allAdvertisementsDto);

		var controller = new AdvertisementsController(serviceStub.Object);

		// Act
		var result = await controller.GetAllAsync();

		// Assert
		result.Should().BeEquivalentTo(allAdvertisementsDto);
	}


	[Fact]
	public async Task GetAllAsync_WithMatchingTitle_ReturnsMachingAdvertisements()
	{
		// Arrange
		var allAdvertisementsDto = new[]
		{
			mapper.Map<AdvertisementDto>(CreateRandomAdvertisement("Trabant ko nov")),
			mapper.Map<AdvertisementDto>(CreateRandomAdvertisement("Reno Clio")),
			mapper.Map<AdvertisementDto>(CreateRandomAdvertisement("Open Astra")),
			mapper.Map<AdvertisementDto>(CreateRandomAdvertisement("Reno Senic")),
			mapper.Map<AdvertisementDto>(CreateRandomAdvertisement("Mitsubishi"))
		};
		string titleToMatch = "Reno";

		serviceStub.Setup(service => service.GetAllAsync(It.IsAny<string>()))
							.ReturnsAsync((string titleToMatch) =>
							allAdvertisementsDto.Where(ad =>
							ad.Title.Contains(titleToMatch, StringComparison.OrdinalIgnoreCase)));

		var controller = new AdvertisementsController(serviceStub.Object);

		// Act
		var result = await controller.GetAllAsync(titleToMatch);

		// Assert
		result.Should().OnlyContain(ad =>
		ad.Title == allAdvertisementsDto[1].Title || ad.Title == allAdvertisementsDto[3].Title);
	}


	[Fact]
	public async Task CreateAsync_WithAdvertisementToCreate_ReturnsCreatedAdvertisement()
	{
		// Arrange
		var createdAdvertisement = CreateRandomAdvertisement();
		var createdAdvertisementDto = mapper.Map<AdvertisementDto>(createdAdvertisement);
		
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
		serviceStub.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateAdvertisementDto>()))
							.ThrowsAsync(new NotFoundException());

		var controller = new AdvertisementsController(serviceStub.Object);

		// Act
		Func<Task> act = async () => await controller.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateAdvertisementDto>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task UpdateAsync_WithExistingAdvertisement_ReturnsNoContent()
	{
		// Arrange
		var updatedAdvertisement = CreateRandomAdvertisement();
		serviceStub.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateAdvertisementDto>()))
							.Returns(Task.CompletedTask);

		var controller = new AdvertisementsController(serviceStub.Object);

		// Act
		var result = await controller.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateAdvertisementDto>());

		// Assert
		result.Should().BeOfType<NoContentResult>();
	}


	[Fact]
	public async Task DeleteAsync_WithUnexistingAdvertisement_ThrowsNotFound()
	{
		// Arrange
		serviceStub.Setup(service => service.DeleteAsync(It.IsAny<int>()))
							.ThrowsAsync(new NotFoundException());

		var controller = new AdvertisementsController(serviceStub.Object);

		// Act
		Func<Task> act = async () => await controller.DeleteAsync(It.IsAny<int>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task DeleteAsync_WithExistingAdvertisement_ReturnsNoContent()
	{
		// Arrange
		var updatedAdvertisement = CreateRandomAdvertisement();
		serviceStub.Setup(service => service.DeleteAsync(It.IsAny<int>()))
							.Returns(Task.CompletedTask);

		var controller = new AdvertisementsController(serviceStub.Object);

		// Act
		var result = await controller.DeleteAsync(It.IsAny<int>());

		// Assert
		result.Should().BeOfType<NoContentResult>();
	}
}