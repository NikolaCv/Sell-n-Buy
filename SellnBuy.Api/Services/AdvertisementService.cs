using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Repositories;

namespace SellnBuy.Api.Services
{
	public class AdvertisementService : BaseService<Advertisement, AdvertisementDto, CreateAdvertisementDto, UpdateAdvertisementDto>
	{
		public AdvertisementService(IRepository<Advertisement> repository) : base(repository)
		{
		}
		
        public override async Task<IEnumerable<AdvertisementDto>> GetAllAsync(string? title = null)
        {
            var advertisements = (await repository.GetAllAsync())
						.Select(advertisement => advertisement.AsDto<Advertisement, AdvertisementDto>());
			
			if (!string.IsNullOrWhiteSpace(title))
				advertisements = advertisements.Where(item => item.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
			
			return advertisements;
        }
    }
}