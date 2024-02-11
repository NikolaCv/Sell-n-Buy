using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Services;

namespace SellnBuy.Api.Controllers;

public class CategoriesController : BaseController<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto>
{
    public CategoriesController(IService<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto> service) : base(service)
    {
    }
}