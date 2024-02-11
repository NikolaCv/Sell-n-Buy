using SellnBuy.Api.Entities.DTOs;

namespace SellnBuy.Api.Entities.Mapper;

public class CategoryMappingProfile : BaseMappingProfile<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto>
{
    public CategoryMappingProfile()
    {
    }
}