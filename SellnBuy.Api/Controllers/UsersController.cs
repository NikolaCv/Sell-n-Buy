using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellnBuy.Api.Entities;
using SellnBuy.Api.Entities.DTOs;
using SellnBuy.Api.Services;

namespace SellnBuy.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
	protected readonly IUsersService service;

	public UsersController(IUsersService service)
	{
		this.service = service;
	}

	// GET /T
	[HttpGet]
	public async Task<IEnumerable<UserDto>> GetAllAsync(string? name = null)
	{
		return await service.GetAllAsync(name);
	}

	// GET /T/{id}
	[HttpGet]
	[Route("{id}")]
	public async Task<UserDto> GetAsync(string id)
	{
		return await service.GetAsync(id);
	}

	// POST /T
	// [HttpPost]
	// public async Task<ActionResult<UserDto>> CreateAsync(CreateUserDto createUserDto)
	// {
	// 	var (createdUserDto, createdId) = await service.CreateAsync(createUserDto);
	// 	return CreatedAtAction(nameof(CreateAsync), new { id = createdId }, createdUserDto);
	// }

	// PUT /T/{id}
	[HttpPut]
	[Route("{id}")]
	public async Task<ActionResult> UpdateAsync(string id, UpdateUserDto updateUserDto)
	{
		await service.UpdateAsync(id, updateUserDto);
		return NoContent();
	}

	// DELETE /T/{id}
	[HttpDelete]
	[Route("{id}")]
	public async Task<ActionResult> DeleteAsync(string id)
	{
		await service.DeleteAsync(id);
		return NoContent();
	}
}