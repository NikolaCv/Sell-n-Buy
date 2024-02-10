using SellnBuy.Api.Entities.DTOs;

namespace SellnBuy.Api.Entities.Mapper;

public class ConditionMappingProfile : BaseMappingProfile<Condition, ConditionDto, CreateConditionDto, UpdateConditionDto>
{
    public ConditionMappingProfile()
    {
    }
}