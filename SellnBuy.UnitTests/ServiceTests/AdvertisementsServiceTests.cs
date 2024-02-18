using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using SellnBuy.Api;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Entities.Mapper;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Repositories;
using SellnBuy.Api.Services;
using Xunit;

namespace SellnBuy.UnitTests.ServiceTests;

public class AdvertisementsServiceTests
{
	private readonly IMapper mapper;
	private readonly Mock<IBaseRepository<Advertisement>> repositoryStub = new();

	private static Advertisement CreateRandomAdvertisement(string? title = null) => new()
	{
		Id = CreateRandomInt(),
		Title = title ?? Guid.NewGuid().ToString(),
		Description = Guid.NewGuid().ToString(),
		Price = CreateRandomInt(),
		ConditionId = CreateRandomInt(),
		UserId = Guid.NewGuid().ToString(),
		CategoryId = CreateRandomInt()
	};

	private static int CreateRandomInt() => new Random().Next(1, 1000);


	public AdvertisementsServiceTests()
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
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
							.ReturnsAsync((Advertisement)null!);

		var service = new AdvertisementsService(repositoryStub.Object, mapper);

		// Act
		Func<Task> act = async () => await service.GetAsync(It.IsAny<int>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task GetAsync_WithExistingAdvertisement_ReturnsExpectedAdvertisement()
	{
		// Arrange
		var existingAdvertisement = CreateRandomAdvertisement();
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
							.ReturnsAsync(existingAdvertisement);

		var service = new AdvertisementsService(repositoryStub.Object, mapper);

		// Act
		var result = await service.GetAsync(It.IsAny<int>());

		// Assert
		result.Should().BeOfType<AdvertisementDto>();
		result.Should().BeEquivalentTo(existingAdvertisement);
	}


	[Fact]
	public async Task GetAllAsync_WithExistingUsers_ReturnsAllUsers()
	{
		// Arrange
		var allAdvertisements = new[]
		{
			CreateRandomAdvertisement(),
			CreateRandomAdvertisement(),
			CreateRandomAdvertisement()
		};
		repositoryStub.Setup(repo => repo.GetAllAsync())
							.ReturnsAsync(allAdvertisements);

		var service = new AdvertisementsService(repositoryStub.Object, mapper);

		// Act
		var result = await service.GetAllAsync();

		// Assert
		result.Should().BeEquivalentTo(allAdvertisements);
	}


	[Fact]
	public async Task GetAllAsync_WithMatchingTitle_ReturnsMatchingAdvertisements()
	{
		// Arrange
		var allAdvertisements = new[]
		{
			CreateRandomAdvertisement("oglas 1"),
			CreateRandomAdvertisement("oglas 2"),
			CreateRandomAdvertisement("glas 2"),
			CreateRandomAdvertisement("glas 3"),
		};
		string nameToMatch = "glas 2";
		repositoryStub.Setup(repo => repo.GetAllAsync())
							.ReturnsAsync(allAdvertisements);

		var service = new AdvertisementsService(repositoryStub.Object, mapper);

		// Act
		var result = await service.GetAllAsync(nameToMatch);

		// Assert
		result.Should().OnlyContain(ad =>
		ad.Title == allAdvertisements[1].Title || ad.Title == allAdvertisements[2].Title);
	}


	[Fact]
	public async Task CreateAsync_WithAdvertisementToCreate_ReturnsCreatedAdvertisementAndCreatedAdvertisementId()
	{
		// Arrange
		var createdAdvertisement = CreateRandomAdvertisement();

		repositoryStub.Setup(repo => repo.CreateAsync(It.IsAny<Advertisement>()))
						.Callback<Advertisement>(advertisement =>
						{
							advertisement.Id = CreateRandomInt();
						})
						.Returns(Task.CompletedTask);

		var service = new AdvertisementsService(repositoryStub.Object, mapper);

		var advertisementDto = mapper.Map<CreateAdvertisementDto>(createdAdvertisement);

		// Act
		var result = await service.CreateAsync(advertisementDto);

		// Assert
		result.Item1.Should().BeEquivalentTo(advertisementDto);
		result.Item1.Id.Should().BeGreaterThan(0);
		result.Item1.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, new TimeSpan(0, 0, 0, 1));
		result.Item2.Should().BeGreaterThan(0);
	}


	[Fact]
	public async Task UpdateAsync_WithUnexistingAdvertisement_ThrowsNotFound()
	{
		// Arrange
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
						.ReturnsAsync((Advertisement)null!);
		repositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<Advertisement>()))
							.Returns(Task.CompletedTask);

		var service = new AdvertisementsService(repositoryStub.Object, mapper);

		// Act
		Func<Task> act = async () => await service.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateAdvertisementDto>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task UpdateAsync_WithExistingAdvertisement_DoesNotThrowException()
	{
		// Arrange
		var existingAdvertisement = CreateRandomAdvertisement();
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
						.ReturnsAsync(existingAdvertisement);
		repositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<Advertisement>()))
							.Returns(Task.CompletedTask);

		var service = new AdvertisementsService(repositoryStub.Object, mapper);

		// Act
		await service.UpdateAsync(It.IsAny<int>(), mapper.Map<UpdateAdvertisementDto>(existingAdvertisement));

		// Assert
		// empty
	}


	[Fact]
	public async Task DeleteAsync_WithUnexistingAdvertisement_ThrowsNotFound()
	{
		// Arrange
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
						.ReturnsAsync((Advertisement)null!);
		repositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<Advertisement>()))
							.Returns(Task.CompletedTask);

		var service = new AdvertisementsService(repositoryStub.Object, mapper);

		// Act
		Func<Task> act = async () => await service.DeleteAsync(It.IsAny<int>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task DeleteAsync_WithExistingAdvertisement_DoesNotThrowException()
	{
		// Arrange
		var existingAdvertisement = CreateRandomAdvertisement();
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
						.ReturnsAsync(existingAdvertisement);
		repositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<Advertisement>()))
							.Returns(Task.CompletedTask);

		var service = new AdvertisementsService(repositoryStub.Object, mapper);

		// Act
		await service.DeleteAsync(It.IsAny<int>());

		// Assert
		// empty
	}
}