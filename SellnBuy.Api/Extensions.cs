
using SellnBuy.Api.Entities;

namespace SellnBuy.Api
{
	public static class Extensions
	{
		public static TDto AsDto<T, TDto>(this T item)
		where TDto : class
		where T : BaseEntity
		{
			switch (item)
			{
				case User user when typeof(TDto) == typeof(UserDto):
					return new UserDto
					(
						user.Id,
						user.Name,
						user.Bio,
						user.PhoneNumber,
						user.Email,
						user.CreatedDate
					) as TDto;
					
				case User user when typeof(TDto) == typeof(CreateUserDto):
					return new CreateUserDto
					(
						user.Name,
						user.Bio,
						user.PhoneNumber,
						user.Email
					) as TDto;
					
				case User user when typeof(TDto) == typeof(UpdateUserDto):
					return new UpdateUserDto
					(
						user.Name,
						user.Bio,
						user.PhoneNumber,
						user.Email
					) as TDto;
					
				case Advertisement advertisement when typeof(TDto) == typeof(AdvertisementDto):
					return new AdvertisementDto
					(
						advertisement.Id,
						advertisement.Title,
						advertisement.Description,
						advertisement.Condition,
						advertisement.Price,
						advertisement.CreatedDate,
						advertisement.userId,
						advertisement.categoryId
					) as TDto;
					
				case Advertisement advertisement when typeof(TDto) == typeof(CreateAdvertisementDto):
					return new CreateAdvertisementDto
					(
						advertisement.Title,
						advertisement.Description,
						advertisement.Condition,
						advertisement.Price,
						advertisement.userId,
						advertisement.categoryId
					) as TDto;
					
				case Advertisement advertisement when typeof(TDto) == typeof(UpdateAdvertisementDto):
					return new UpdateAdvertisementDto
					(
						advertisement.Title,
						advertisement.Description,
						advertisement.Condition,
						advertisement.Price,
						advertisement.userId,
						advertisement.categoryId
					) as TDto;
					
				default:
					return null;
			};
		}
		
		public static T AsEntity<T, TDto>(this TDto itemDto, Guid id, DateTimeOffset createdDate)
		where TDto : class
		where T : BaseEntity
		{
			return itemDto switch
			{
				CreateUserDto userDto => new User
				{
					Id = id,
					Name = userDto.Name,
					Bio = userDto.Bio,
					PhoneNumber = userDto.PhoneNumber,
					Email = userDto.Email,
					CreatedDate = createdDate
				} as T,
				
				UpdateUserDto userDto => new User
				{
					Id = id,
					Name = userDto.Name,
					Bio = userDto.Bio,
					PhoneNumber = userDto.PhoneNumber,
					Email = userDto.Email,
					CreatedDate = createdDate
				} as T,
				
				CreateAdvertisementDto advertisementDto => new Advertisement
				{
					Id = id,
					Title = advertisementDto.Title,
					Description = advertisementDto.Description,
					Condition = advertisementDto.Condition,
					Price = advertisementDto.Price,
					CreatedDate = createdDate,
					userId = advertisementDto.userId,
					categoryId = advertisementDto.categoryId
				 } as T,	
				 			
				UpdateAdvertisementDto advertisementDto => new Advertisement
				{
					Id = id,
					Title = advertisementDto.Title,
					Description = advertisementDto.Description,
					Condition = advertisementDto.Condition,
					Price = advertisementDto.Price,
					CreatedDate = createdDate,
					userId = advertisementDto.userId,
					categoryId = advertisementDto.categoryId
				 } as T,
				 
				_ => throw new NotSupportedException($"Type {typeof(TDto).FullName} not supported")
				
			};
		}
	}

}