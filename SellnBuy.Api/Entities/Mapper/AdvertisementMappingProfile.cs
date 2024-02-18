using SellnBuy.Api.Entities.DTOs;

namespace SellnBuy.Api.Entities.Mapper;

public class AdvertisementMappingProfile : BaseMappingProfile<Advertisement, AdvertisementDto, CreateAdvertisementDto, UpdateAdvertisementDto>
{
	public AdvertisementMappingProfile()
	{
		// CreateMap<Advertisement, AdvertisementDto>()
		// 	.ForCtorParam("UserDto", opt => opt.MapFrom(src => src.User))
		// 	.ForCtorParam("ConditionDto", opt => opt.MapFrom(src => src.Condition))
		// 	.ForCtorParam("CategoryDto", opt => opt.MapFrom(src => src.Category));
	}
}