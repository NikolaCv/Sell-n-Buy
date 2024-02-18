using SellnBuy.Api.Entities.DTOs;

namespace SellnBuy.Api.Entities.Mapper;

public class UserMappingProfile : BaseMappingProfile<User, UserDto, CreateUserDto, UpdateUserDto>
{
    public UserMappingProfile()
    {
        CreateMap<RegisterUserDto, User>();
        CreateMap<LoginUserDto, User>();
    }
}