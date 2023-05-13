using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Repositories;

namespace SellnBuy.Api.Services
{
	public class UserService : BaseService<User, UserDto, CreateUserDto, UpdateUserDto>
	{
		public UserService(IRepository<User> repository) : base(repository)
		{
		}
		
        public override async Task<IEnumerable<UserDto>> GetAllAsync(string? name = null)
        {
            var users = (await repository.GetAllAsync())
						.Select(user => user.AsDto<User, UserDto>());
			
			if (!string.IsNullOrWhiteSpace(name))
				users = users.Where(user => user.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
			
			return users;
        }
    }
}