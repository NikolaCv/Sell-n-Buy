using Microsoft.AspNetCore.Mvc;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Services;

namespace SellnBuy.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ConditionsController : BaseController<Condition, ConditionDto, CreateConditionDto, UpdateConditionDto>
{
    public ConditionsController(IBaseService<Condition, ConditionDto, CreateConditionDto, UpdateConditionDto> service) : base(service)
    {
    }
}