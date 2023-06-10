using Microsoft.AspNetCore.Mvc;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Services;

namespace SellnBuy.Api.Controllers
{	
	[ApiController]
	[Route("[controller]")]
	public abstract class BaseController<T, TDto, CreateTDto, UpdateTDto> : ControllerBase
	where T : BaseEntity
	where TDto : class
	where CreateTDto : class
	where UpdateTDto : class
	{
		protected readonly IService<T, TDto, CreateTDto, UpdateTDto> service;

		public BaseController(IService<T, TDto, CreateTDto, UpdateTDto> service)
		{
			this.service = service;
		}
		
		// GET /T
		[HttpGet]
		public async Task<IEnumerable<TDto>> GetAllAsync(string? searchTerm = null)
		{
			return await service.GetAllAsync(searchTerm);
		}
		
		// GET /T/{id}
		[HttpGet]
		[Route("{id}")]
		public async Task<TDto> GetAsync(Guid id)
		{	
			return await service.GetAsync(id);
		}
		
		// POST /T
		[HttpPost]
		public async Task<ActionResult<TDto>> CreateAsync(CreateTDto itemDto)
		{
			var (createdItemDto, createdId) = await service.CreateAsync(itemDto);
			return CreatedAtAction(nameof(CreateAsync), new { id = createdId }, createdItemDto);
		}
		
		// PUT /T/{id}
		[HttpPut]
		[Route("{id}")]
		public async Task<ActionResult> UpdateAsync(Guid id, UpdateTDto itemDto)
		{
			await service.UpdateAsync(id, itemDto);
			return NoContent();
		}
		
		// DELETE /T/{id}
		[HttpDelete]
		[Route("{id}")]
		public async Task<ActionResult> DeleteAsync(Guid id)
		{
			await service.DeleteAsync(id);
			return NoContent();
		}
		
	}
}