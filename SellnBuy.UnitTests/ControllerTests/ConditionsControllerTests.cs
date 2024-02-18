using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;
using SellnBuy.Api.Controllers;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Entities.Mapper;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Services;
using Xunit;

namespace SellnBuy.UnitTests.ControllerTests;

public class ConditionsControllerTests
{
    private readonly IMapper mapper;
    private readonly Mock<IBaseService<Condition, ConditionDto, CreateConditionDto, UpdateConditionDto>> serviceStub = new();

    private static Condition CreateRandomCondition(string? name = null)
    {
        return new()
        {
            Id = new Random().Next(1, 1000),
            Name = name ?? Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            CreatedDate = DateTimeOffset.UtcNow
        };
    }

    public ConditionsControllerTests()
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
        serviceStub.Setup(service => service.GetAsync(It.IsAny<int>()))
                        .ThrowsAsync(new NotFoundException());

        var controller = new ConditionsController(serviceStub.Object);

        // Act
        Func<Task> act = async () => await controller.GetAsync(It.IsAny<int>());

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }


    [Fact]
    public async Task GetAsync_WithExistingCondition_ReturnsExpectedCondition()
    {
        // Arrange
        var expectedConditionDto = mapper.Map<ConditionDto>(CreateRandomCondition());
        serviceStub.Setup(service => service.GetAsync(It.IsAny<int>()))
                        .ReturnsAsync(expectedConditionDto);

        var controller = new ConditionsController(serviceStub.Object);

        // Act
        var result = await controller.GetAsync(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<ConditionDto>();
        result.Should().BeEquivalentTo(expectedConditionDto);
    }


    [Fact]
    public async Task GetAllAsync_WithExistingConditions_ReturnsAllConditions()
    {
        // Arrange
        var allConditionsDto = new[]
        {
            mapper.Map<ConditionDto>(CreateRandomCondition()),
            mapper.Map<ConditionDto>(CreateRandomCondition()),
            mapper.Map<ConditionDto>(CreateRandomCondition()),
        };
        serviceStub.Setup(service => service.GetAllAsync(It.IsAny<string>()))
                        .ReturnsAsync(allConditionsDto);

        var controller = new ConditionsController(serviceStub.Object);

        // Act
        var result = await controller.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(allConditionsDto);
    }


    [Fact]
    public async Task GetAllAsync_WithMatchingName_ReturnsMachingConditions()
    {
        // Arrange
        var allConditionsDto = new[]
        {
            mapper.Map<ConditionDto>(CreateRandomCondition("New")),
            mapper.Map<ConditionDto>(CreateRandomCondition("Broken")),
            mapper.Map<ConditionDto>(CreateRandomCondition("Almost new")),
            mapper.Map<ConditionDto>(CreateRandomCondition("Damaged"))
        };
        string nameToMatch = "new";

        serviceStub.Setup(service => service.GetAllAsync(It.IsAny<string>()))
                            .ReturnsAsync((string nameToMatch) =>
                            allConditionsDto.Where(Condition =>
                            Condition.Name.Contains(nameToMatch, StringComparison.OrdinalIgnoreCase)));

        var controller = new ConditionsController(serviceStub.Object);

        // Act
        var result = await controller.GetAllAsync(nameToMatch);

        // Assert
        result.Should().OnlyContain(condition =>
        condition.Name == allConditionsDto[0].Name || condition.Name == allConditionsDto[2].Name);
    }


    [Fact]
    public async Task CreateAsync_WithConditionToCreate_ReturnsCreatedCondition()
    {
        // Arrange
        var createdCondition = CreateRandomCondition();
        var createdConditionDto = mapper.Map<ConditionDto>(createdCondition);

        serviceStub.Setup(service => service.CreateAsync(It.IsAny<CreateConditionDto>()))
                            .ReturnsAsync((createdConditionDto, createdCondition.Id));

        var controller = new ConditionsController(serviceStub.Object);

        // Act
        var result = await controller.CreateAsync(It.IsAny<CreateConditionDto>());

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult?.Value.Should().BeEquivalentTo(createdConditionDto);
    }


    [Fact]
    public async Task UpdateAsync_WithUnexistingCondition_ThrowsNotFound()
    {
        // Arrange
        serviceStub.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateConditionDto>()))
                            .ThrowsAsync(new NotFoundException());

        var controller = new ConditionsController(serviceStub.Object);

        // Act
        // Delay function execution till assert section with act
        Func<Task> act = async () => await controller.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateConditionDto>());

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }


    [Fact]
    public async Task UpdateAsync_WithExistingCondition_ReturnsNoContent()
    {
        // Arrange
        serviceStub.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateConditionDto>()))
                            .Returns(Task.CompletedTask);

        var controller = new ConditionsController(serviceStub.Object);

        // Act
        var result = await controller.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateConditionDto>());

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }


    [Fact]
    public async Task DeleteAsync_WithUnexistingCondition_ThrowsNotFound()
    {
        // Arrange
        serviceStub.Setup(service => service.DeleteAsync(It.IsAny<int>()))
                            .ThrowsAsync(new NotFoundException());

        var controller = new ConditionsController(serviceStub.Object);

        // Act
        // Delay function execution till assert section with act
        Func<Task> act = async () => await controller.DeleteAsync(It.IsAny<int>());

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }


    [Fact]
    public async Task DeleteAsync_WithExistingCondition_ReturnsNoContent()
    {
        // Arrange
        serviceStub.Setup(service => service.DeleteAsync(It.IsAny<int>()))
                            .Returns(Task.CompletedTask);

        var controller = new ConditionsController(serviceStub.Object);

        // Act
        var result = await controller.DeleteAsync(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}
