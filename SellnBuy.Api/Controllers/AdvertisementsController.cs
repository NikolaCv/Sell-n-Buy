using Microsoft.AspNetCore.Mvc;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Repositories;
using SellnBuy.Api.Services;

namespace SellnBuy.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AdvertisementsController : Controller<Advertisement, AdvertisementDto, CreateAdvertisementDto, UpdateAdvertisementDto>
	{
		public AdvertisementsController(IService<Advertisement, AdvertisementDto, CreateAdvertisementDto, UpdateAdvertisementDto> service) : base(service)
		{
		}
	}
}