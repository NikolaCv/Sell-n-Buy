using Microsoft.AspNetCore.Mvc;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Services;

namespace SellnBuy.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AdvertisementsController : BaseController<Advertisement, AdvertisementDto, CreateAdvertisementDto, UpdateAdvertisementDto>
{
	public AdvertisementsController(IBaseService<Advertisement, AdvertisementDto, CreateAdvertisementDto, UpdateAdvertisementDto> service) : base(service)
	{
	}
}
