using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Services;

namespace SellnBuy.Api.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class BaseController<T, TDto, CreateTDto, UpdateTDto> : ControllerBase
{
	protected readonly IBaseService<T, TDto, CreateTDto, UpdateTDto> service;

	public BaseController(IBaseService<T, TDto, CreateTDto, UpdateTDto> service)
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
	public async Task<TDto> GetAsync(int id)
	{
		return await service.GetAsync(id);
	}

	// POST /T
	[HttpPost]
	public async Task<ActionResult<TDto>> CreateAsync(CreateTDto createItemDto)
	{
		var (createdItemDto, createdId) = await service.CreateAsync(createItemDto);
		return CreatedAtAction(nameof(CreateAsync), new { id = createdId }, createdItemDto);
	}

	// PUT /T/{id}
	[HttpPut]
	[Route("{id}")]
	public async Task<ActionResult> UpdateAsync(int id, UpdateTDto updateItemDto)
	{
		await service.UpdateAsync(id, updateItemDto);
		return NoContent();
	}

	// DELETE /T/{id}
	[HttpDelete]
	[Route("{id}")]
	public async Task<ActionResult> DeleteAsync(int id)
	{
		await service.DeleteAsync(id);
		return NoContent();
	}

}