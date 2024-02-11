using AutoMapper;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Repositories;

namespace SellnBuy.Api.Services;

public class ConditionsService : BaseService<Condition, ConditionDto, CreateConditionDto, UpdateConditionDto>
{
	public ConditionsService(IRepository<Condition> repository, IMapper mapper) : base(repository, mapper)
	{
	}

	public async override Task<IEnumerable<ConditionDto>> GetAllAsync(string? name = null)
	{
		var conditions = await repository.GetAllAsync();

		if (!string.IsNullOrWhiteSpace(name))
			conditions = conditions.Where(user => user.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

		return conditions.Select(mapper.Map<ConditionDto>);
	}
}