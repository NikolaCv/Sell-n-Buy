using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Repositories;

namespace SellnBuy.Api.Services
{
	public class UsersService : BaseService<User, UserDto, CreateUserDto, UpdateUserDto>
	{
		public UsersService(IRepository<User> repository) : base(repository)
		{
		}
		
        public override async Task<IEnumerable<UserDto>> GetAllAsync(string? name = null)
        {
            var users = await repository.GetAllAsync();
			
			if (!string.IsNullOrWhiteSpace(name))
				users = users.Where(user => user.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
			
			return users.Select(user => user.AsDto<User, UserDto>());
        }
    }
}