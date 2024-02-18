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

public class CategoriesControllerTests
{
    private readonly IMapper mapper;
    private readonly Mock<IBaseService<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto>> serviceStub = new();

    private static Category CreateRandomCategory(string? name = null)
    {
        return new()
        {
            Id = new Random().Next(1, 1000),
            Name = name ?? Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            CreatedDate = DateTimeOffset.UtcNow
        };
    }

    public CategoriesControllerTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CategoryMappingProfile>();
        });

        mapper = config.CreateMapper();
    }


    [Fact]
    public async Task GetAsync_WithUnexistingCategory_ThrowsNotFound()
    {
        // Arrange
        serviceStub.Setup(service => service.GetAsync(It.IsAny<int>()))
                        .ThrowsAsync(new NotFoundException());

        var controller = new CategoriesController(serviceStub.Object);

        // Act
        Func<Task> act = async () => await controller.GetAsync(It.IsAny<int>());

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }


    [Fact]
    public async Task GetAsync_WithExistingCategory_ReturnsExpectedCategory()
    {
        // Arrange
        var expectedCategoryDto = mapper.Map<CategoryDto>(CreateRandomCategory());
        serviceStub.Setup(service => service.GetAsync(It.IsAny<int>()))
                        .ReturnsAsync(expectedCategoryDto);

        var controller = new CategoriesController(serviceStub.Object);

        // Act
        var result = await controller.GetAsync(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<CategoryDto>();
        result.Should().BeEquivalentTo(expectedCategoryDto);
    }


    [Fact]
    public async Task GetAllAsync_WithExistingCategories_ReturnsAllCategories()
    {
        // Arrange
        var allCategoriesDto = new[]
        {
            mapper.Map<CategoryDto>(CreateRandomCategory()),
            mapper.Map<CategoryDto>(CreateRandomCategory()),
            mapper.Map<CategoryDto>(CreateRandomCategory()),
        };
        serviceStub.Setup(service => service.GetAllAsync(It.IsAny<string>()))
                        .ReturnsAsync(allCategoriesDto);

        var controller = new CategoriesController(serviceStub.Object);

        // Act
        var result = await controller.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(allCategoriesDto);
    }


    [Fact]
    public async Task GetAllAsync_WithMatchingName_ReturnsMachingCategories()
    {
        // Arrange
        var allCategoriesDto = new[]
        {
            mapper.Map<CategoryDto>(CreateRandomCategory("Cars")),
            mapper.Map<CategoryDto>(CreateRandomCategory("Entertainment")),
            mapper.Map<CategoryDto>(CreateRandomCategory("Other")),
            mapper.Map<CategoryDto>(CreateRandomCategory("Fast cars"))
        };
        string nameToMatch = "car";

        serviceStub.Setup(service => service.GetAllAsync(It.IsAny<string>()))
                            .ReturnsAsync((string nameToMatch) =>
                            allCategoriesDto.Where(Category =>
                            Category.Name.Contains(nameToMatch, StringComparison.OrdinalIgnoreCase)));

        var controller = new CategoriesController(serviceStub.Object);

        // Act
        var result = await controller.GetAllAsync(nameToMatch);

        // Assert
        result.Should().OnlyContain(category =>
        category.Name == allCategoriesDto[0].Name || category.Name == allCategoriesDto[3].Name);
    }


    [Fact]
    public async Task CreateAsync_WithCategoryToCreate_ReturnsCreatedCategory()
    {
        // Arrange
        var createdCategory = CreateRandomCategory();
        var createdCategoryDto = mapper.Map<CategoryDto>(createdCategory);

        serviceStub.Setup(service => service.CreateAsync(It.IsAny<CreateCategoryDto>()))
                            .ReturnsAsync((createdCategoryDto, createdCategory.Id));

        var controller = new CategoriesController(serviceStub.Object);

        // Act
        var result = await controller.CreateAsync(It.IsAny<CreateCategoryDto>());

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult?.Value.Should().BeEquivalentTo(createdCategoryDto);
    }


    [Fact]
    public async Task UpdateAsync_WithUnexistingCategory_ThrowsNotFound()
    {
        // Arrange
        serviceStub.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateCategoryDto>()))
                            .ThrowsAsync(new NotFoundException());

        var controller = new CategoriesController(serviceStub.Object);

        // Act
        // Delay function execution till assert section with act
        Func<Task> act = async () => await controller.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateCategoryDto>());

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }


    [Fact]
    public async Task UpdateAsync_WithExistingCategory_ReturnsNoContent()
    {
        // Arrange
        serviceStub.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateCategoryDto>()))
                            .Returns(Task.CompletedTask);

        var controller = new CategoriesController(serviceStub.Object);

        // Act
        var result = await controller.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateCategoryDto>());

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }


    [Fact]
    public async Task DeleteAsync_WithUnexistingCategory_ThrowsNotFound()
    {
        // Arrange
        serviceStub.Setup(service => service.DeleteAsync(It.IsAny<int>()))
                            .ThrowsAsync(new NotFoundException());

        var controller = new CategoriesController(serviceStub.Object);

        // Act
        // Delay function execution till assert section with act
        Func<Task> act = async () => await controller.DeleteAsync(It.IsAny<int>());

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }


    [Fact]
    public async Task DeleteAsync_WithExistingCategory_ReturnsNoContent()
    {
        // Arrange
        serviceStub.Setup(service => service.DeleteAsync(It.IsAny<int>()))
                            .Returns(Task.CompletedTask);

        var controller = new CategoriesController(serviceStub.Object);

        // Act
        var result = await controller.DeleteAsync(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}
