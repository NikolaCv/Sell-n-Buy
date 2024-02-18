using FluentAssertions;
using Moq;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Entities.Mapper;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Repositories;
using Xunit;
using AutoMapper;
using SellnBuy.Api.Services;

namespace SellnBuy.UnitTests.ServiceTests;

public class CategoriesServiceTests
{
    private readonly IMapper mapper;
    private readonly Mock<IBaseRepository<Category>> repositoryStub = new();

    private static Category CreateRandomCategory(string? name = null) => new()
    {
        Id = CreateRandomInt(),
        Name = name ?? Guid.NewGuid().ToString(),
        Description = Guid.NewGuid().ToString(),
        CreatedDate = DateTimeOffset.UtcNow
    };


    private static int CreateRandomInt() => new Random().Next(1, 1000);


    public CategoriesServiceTests()
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
        repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((Category)null!);

        var service = new CategoriesService(repositoryStub.Object, mapper);

        // Act
        // Delay function execution till assert section with act
        Func<Task> act = async () => await service.GetAsync(It.IsAny<int>());

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }


    [Fact]
    public async Task GetAsync_WithExistingCategory_ReturnsExpectedCategory()
    {
        // Arange
        var expectedCategory = CreateRandomCategory();
        repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                            .ReturnsAsync(expectedCategory);

        var service = new CategoriesService(repositoryStub.Object, mapper);

        // Act
        var result = await service.GetAsync(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<CategoryDto>();
        result.Should().BeEquivalentTo(expectedCategory);
    }


    [Fact]
    public async Task GetAllAsync_WithExistingCategories_ReturnsAllCategories()
    {
        // Arange
        var allCategories = new[]
        {
            CreateRandomCategory(),
            CreateRandomCategory(),
            CreateRandomCategory()
        };
        repositoryStub.Setup(repo => repo.GetAllAsync())
                            .ReturnsAsync(allCategories);

        var service = new CategoriesService(repositoryStub.Object, mapper);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(allCategories);
    }


    [Fact]
    public async Task GetAllAsync_WithMatchingName_ReturnsMatchingCategories()
    {
        // Arange
        var allCategories = new[]
        {
            CreateRandomCategory("New"),
            CreateRandomCategory("Broken"),
            CreateRandomCategory("Almost new"),
            CreateRandomCategory("Damaged")
        };
        string nameToMatch = "New";

        repositoryStub.Setup(repo => repo.GetAllAsync())
                            .ReturnsAsync(allCategories);

        var service = new CategoriesService(repositoryStub.Object, mapper);

        // Act
        var result = await service.GetAllAsync(nameToMatch);

        // Assert
        result.Should().OnlyContain(category =>
        category.Name == allCategories[0].Name || category.Name == allCategories[2].Name);
    }


    [Fact]
    public async Task CreateAsync_WithCategoryToCreate_ReturnsCreatedCategoryDtoAndCreatedCategoryId()
    {
        // Arange
        var createdCategory = CreateRandomCategory();

        repositoryStub.Setup(repo => repo.CreateAsync(It.IsAny<Category>()))
                            .Callback<Category>(Category =>
                            {
                                Category.Id = CreateRandomInt();
                            })
                            .Returns(Task.CompletedTask);

        var service = new CategoriesService(repositoryStub.Object, mapper);

        var CategoryDto = mapper.Map<CreateCategoryDto>(createdCategory);

        // Act
        var result = await service.CreateAsync(CategoryDto);

        // Assert
        result.Item1.Should().BeEquivalentTo(CategoryDto);
        result.Item1.Id.Should().BeGreaterThan(0);
        result.Item1.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, new TimeSpan(0, 0, 0, 1));
        result.Item2.Should().BeGreaterThan(0);
    }


    [Fact]
    public async Task UpdateAsync_WithUnexistingCategory_ThrowsNotFound()
    {
        // Arange
        repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                        .ReturnsAsync((Category)null!);
        repositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<Category>()))
                        .Returns(Task.CompletedTask);

        var service = new CategoriesService(repositoryStub.Object, mapper);

        // Act
        // Delay function execution till assert section with act
        Func<Task> act = async () => await service.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateCategoryDto>());

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }


    [Fact]
    public async Task UpdateAsync_WithExistingCategory_DoesNotThrowException()
    {
        // Arange
        var existingCategory = CreateRandomCategory();
        repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                        .ReturnsAsync(existingCategory);
        repositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<Category>()))
                        .Returns(Task.CompletedTask);

        var service = new CategoriesService(repositoryStub.Object, mapper);

        // Act			
        await service.UpdateAsync(It.IsAny<int>(), mapper.Map<UpdateCategoryDto>(existingCategory));

        // Assert
        // service.UpdateAsync() returns void, test will fail if it throws 
        // If it doesn't throw there is no need to check
    }


    [Fact]
    public async Task DeleteAsync_WithUnexistingCategory_ThrowsNotFound()
    {
        // Arange
        repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                        .ReturnsAsync((Category)null!);

        var service = new CategoriesService(repositoryStub.Object, mapper);

        // Act		
        // Delay function execution till assert section with act
        Func<Task> act = async () => await service.DeleteAsync(It.IsAny<int>());

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }


    [Fact]
    public async Task DeleteAsync_WithExistingCategory_DoesNotThrowException()
    {
        // Arange
        var existingCategory = CreateRandomCategory();
        repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                        .ReturnsAsync(existingCategory);
        repositoryStub.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                        .Returns(Task.CompletedTask);

        var service = new CategoriesService(repositoryStub.Object, mapper);

        // Act			
        await service.DeleteAsync(It.IsAny<int>());

        // Assert
        // service.DeleteAsync() returns void, test will fail if it throws 
        // If it doesn't throw there is no need to check
    }
}