using CustomerService.Application.DTOs;
using CustomerService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.API.Controllers
{
	[ApiController]
	[Route("api/clientes")]
	public class ClientesController : ControllerBase
	{
		private readonly IClienteService _clienteService;

		public ClientesController(
			IClienteService clienteService)
		{
			_clienteService = clienteService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(
				await _clienteService.GetAllAsync());
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			return Ok(
				await _clienteService.GetByIdAsync(id));
		}

		[HttpPost]
		public async Task<IActionResult> Create(
			CreateClienteDto dto)
		{
			var cliente =
				await _clienteService.CreateAsync(dto);

			return CreatedAtAction(
				nameof(GetById),
				new { id = cliente.ClienteId },
				cliente);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update(
			int id,
			UpdateClienteDto dto)
		{
			return Ok(
				await _clienteService.UpdateAsync(
					id,
					dto));
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _clienteService.DeleteAsync(id);

			return NoContent();
		}
	}
}