using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Customer.Services;
using Customer.Models;

namespace Customer.Controllers;

[Controller]
[Route("api/customers")]
public class CustomerController : Controller
{

	private readonly MongoDBService _mongoDBService;
	private static readonly Regex sWhitespace = new(@"\s+");

	public CustomerController(MongoDBService mongoDBService)
	{
		_mongoDBService = mongoDBService;
	}

	[HttpGet]
	public async Task<List<CustomerModel>> Get()
	{
		return await _mongoDBService.GetAsync();
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] CustomerCreateDTO payload)
	{
		CustomerModel customer = new()
		{
			Email = payload.Email,
			FirstName = payload.FirstName,
			LastName = payload.LastName,
			Country = payload.Country,
			State = payload.State,
			City = payload.City,
			Password = Hashing.HashPassword(sWhitespace.Replace(payload.Password, "")),
		};

		await _mongoDBService.CreateAsync(customer);
		return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(string id)
	{
		CustomerModel customer = await _mongoDBService.GetByIdAsync(id);
		if (customer == null)
		{
			return NotFound();
		}

		return Ok(customer);
	}

	[HttpPatch("{id}")]
	public async Task<IActionResult> Patch(string id, [FromBody] CustomeUpdateDTO payload)
	{
		CustomerModel customer = new()
		{
			FirstName = payload.FirstName,
			LastName = payload.LastName,
			Country = payload.Country,
			State = payload.State,
			City = payload.City,
		};

		await _mongoDBService.UpdateAsync(id, customer);
		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		await _mongoDBService.DeleteAsync(id);
		return NoContent();
	}

}