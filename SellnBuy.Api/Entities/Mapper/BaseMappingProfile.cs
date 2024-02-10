using AutoMapper;

namespace SellnBuy.Api.Entities.Mapper
{
	public class BaseMappingProfile<T, TDto, CreateTDto, UpdateTDto> : Profile
	{
		public BaseMappingProfile()
		{
			CreateMap<T, TDto>();
			CreateMap<T, CreateTDto>();
			CreateMap<T, UpdateTDto>();
			CreateMap<CreateTDto, T>();
			CreateMap<UpdateTDto, T>();
		}
	}
}