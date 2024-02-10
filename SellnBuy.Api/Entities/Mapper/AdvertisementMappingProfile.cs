using SellnBuy.Api.Entities.DTOs;

namespace SellnBuy.Api.Entities.Mapper;

public class AdvertisementMappingProfile : BaseMappingProfile<Advertisement, AdvertisementDto, CreateAdvertisementDto, UpdateAdvertisementDto>
{
    public AdvertisementMappingProfile()
    {
    }
}