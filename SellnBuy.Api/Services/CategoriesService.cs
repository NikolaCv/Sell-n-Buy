using AutoMapper;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Repositories;

namespace SellnBuy.Api.Services;

public class CategoriesService : BaseService<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto>
{
    public CategoriesService(IBaseRepository<Category> repository, IMapper mapper) : base(repository, mapper)
    {
    }

    public async override Task<IEnumerable<CategoryDto>> GetAllAsync(string? name = null)
    {
        var categories = await repository.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(name))
            categories = categories.Where(user => user.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

        return categories.Select(mapper.Map<CategoryDto>);
    }
}