using FluentAssertions;
using Moq;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Entities.Mapper;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Repositories;
using SellnBuy.Api.Services;
using Xunit;
using AutoMapper;

namespace SellnBuy.UnitTests.ServiceTests;

public class ConditionsServiceTests
{
	private readonly IMapper mapper;
	private readonly Mock<IRepository<Condition>> repositoryStub = new();

	private static Condition CreateRandomCondition(string? name = null) => new()
	{
		Id = CreateRandomInt(),
		Name = name ?? Guid.NewGuid().ToString(),
		Description = Guid.NewGuid().ToString(),
		CreatedDate = DateTimeOffset.UtcNow
	};


	private static int CreateRandomInt() => new Random().Next(1, 1000);


	public ConditionsServiceTests()
	{
		var config = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile<ConditionMappingProfile>();
		});

		mapper = config.CreateMapper();
	}


	[Fact]
	public async Task GetAsync_WithUnexistingCondition_ThrowsNotFound()
	{
		// Arrange
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
				.ReturnsAsync((Condition)null!);

		var service = new ConditionsService(repositoryStub.Object, mapper);

		// Act
		// Delay function execution till assert section with act
		Func<Task> act = async () => await service.GetAsync(It.IsAny<int>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task GetAsync_WithExistingCondition_ReturnsExpectedCondition()
	{
		// Arange
		var expectedCondition = CreateRandomCondition();
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
							.ReturnsAsync(expectedCondition);

		var service = new ConditionsService(repositoryStub.Object, mapper);

		// Act
		var result = await service.GetAsync(It.IsAny<int>());

		// Assert
		result.Should().BeOfType<ConditionDto>();
		result.Should().BeEquivalentTo(expectedCondition);
	}


	[Fact]
	public async Task GetAllAsync_WithExistingConditions_ReturnsAllConditions()
	{
		// Arange
		var allConditions = new[]
		{
			CreateRandomCondition(),
			CreateRandomCondition(),
			CreateRandomCondition()
		};
		repositoryStub.Setup(repo => repo.GetAllAsync())
							.ReturnsAsync(allConditions);

		var service = new ConditionsService(repositoryStub.Object, mapper);

		// Act
		var result = await service.GetAllAsync();

		// Assert
		result.Should().BeEquivalentTo(allConditions);
	}


	[Fact]
	public async Task GetAllAsync_WithMatchingName_ReturnsMatchingConditions()
	{
		// Arange
		var allConditions = new[]
		{
			CreateRandomCondition("New"),
			CreateRandomCondition("Broken"),
			CreateRandomCondition("Almost new"),
			CreateRandomCondition("Damaged")
		};
		string nameToMatch = "New";

		repositoryStub.Setup(repo => repo.GetAllAsync())
							.ReturnsAsync(allConditions);

		var service = new ConditionsService(repositoryStub.Object, mapper);

		// Act
		var result = await service.GetAllAsync(nameToMatch);

		// Assert
		result.Should().OnlyContain(condition =>
		condition.Name == allConditions[0].Name || condition.Name == allConditions[2].Name);
	}


	[Fact]
	public async Task CreateAsync_WithConditionToCreate_ReturnsCreatedConditionDtoAndCreatedConditionId()
	{
		// Arange
		var createdCondition = CreateRandomCondition();

		repositoryStub.Setup(repo => repo.CreateAsync(It.IsAny<Condition>()))
							.Callback<Condition>(Condition =>
							{
								Condition.Id = CreateRandomInt();
							})
							.Returns(Task.CompletedTask);

		var service = new ConditionsService(repositoryStub.Object, mapper);

		var ConditionDto = mapper.Map<CreateConditionDto>(createdCondition);

		// Act
		var result = await service.CreateAsync(ConditionDto);

		// Assert
		result.Item1.Should().BeEquivalentTo(ConditionDto);
		result.Item1.Id.Should().BeGreaterThan(0);
		result.Item1.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, new TimeSpan(0, 0, 0, 1));
		result.Item2.Should().BeGreaterThan(0);
	}


	[Fact]
	public async Task UpdateAsync_WithUnexistingCondition_ThrowsNotFound()
	{
		// Arange
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
						.ReturnsAsync((Condition)null!);
		repositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<Condition>()))
						.Returns(Task.CompletedTask);

		var service = new ConditionsService(repositoryStub.Object, mapper);

		// Act
		// Delay function execution till assert section with act
		Func<Task> act = async () => await service.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateConditionDto>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task UpdateAsync_WithExistingCondition_DoesNotThrowException()
	{
		// Arange
		var existingCondition = CreateRandomCondition();
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
						.ReturnsAsync(existingCondition);
		repositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<Condition>()))
						.Returns(Task.CompletedTask);

		var service = new ConditionsService(repositoryStub.Object, mapper);

		// Act			
		await service.UpdateAsync(It.IsAny<int>(), mapper.Map<UpdateConditionDto>(existingCondition));

		// Assert
		// service.UpdateAsync() returns void, test will fail if it throws 
		// If it doesn't throw there is no need to check
	}


	[Fact]
	public async Task DeleteAsync_WithUnexistingCondition_ThrowsNotFound()
	{
		// Arange
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
						.ReturnsAsync((Condition)null!);

		var service = new ConditionsService(repositoryStub.Object, mapper);

		// Act		
		// Delay function execution till assert section with act
		Func<Task> act = async () => await service.DeleteAsync(It.IsAny<int>());

		// Assert
		await act.Should().ThrowAsync<NotFoundException>();
	}


	[Fact]
	public async Task DeleteAsync_WithExistingCondition_DoesNotThrowException()
	{
		// Arange
		var existingCondition = CreateRandomCondition();
		repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
						.ReturnsAsync(existingCondition);
		repositoryStub.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
						.Returns(Task.CompletedTask);

		var service = new ConditionsService(repositoryStub.Object, mapper);

		// Act			
		await service.DeleteAsync(It.IsAny<int>());

		// Assert
		// service.DeleteAsync() returns void, test will fail if it throws 
		// If it doesn't throw there is no need to check
	}
}