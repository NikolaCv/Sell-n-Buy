using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SellnBuy.Api.Entities;

namespace SellnBuy.Api.Services;

public interface IService<T, TDto, CreateTDto, UpdateTDto>
where T : BaseEntity
where TDto : class
where CreateTDto : class
where UpdateTDto : class
{
	Task<IEnumerable<TDto>> GetAllAsync(string? searchTerm = null);
	Task<TDto> GetAsync(int id);
	Task<(TDto, int)> CreateAsync(CreateTDto item);
	Task DeleteAsync(int id);
	Task UpdateAsync(int id, UpdateTDto itemDto);
}