using AutoMapper;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Exceptions;
using SellnBuy.Api.Repositories;

namespace SellnBuy.Api.Services;

public class AdvertisementsService : BaseService<Advertisement, AdvertisementDto, CreateAdvertisementDto, UpdateAdvertisementDto>
{
	public AdvertisementsService(IBaseRepository<Advertisement> repository, IMapper mapper) : base(repository, mapper)
	{
	}

	public override async Task<IEnumerable<AdvertisementDto>> GetAllAsync(string? title = null)
	{
		var advertisements = await repository.GetAllAsync();

		if (!string.IsNullOrWhiteSpace(title))
			advertisements = advertisements.Where(item => item.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

		return advertisements.Select(mapper.Map<Advertisement, AdvertisementDto>);
	}
}