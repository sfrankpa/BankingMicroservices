using AccountService.Application.DTOs;
using AccountService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.API.Controllers
{
	[ApiController]
	[Route("api/cuentas")]
	public class CuentasController : ControllerBase
	{
		private readonly ICuentaService _cuentaService;

		public CuentasController(ICuentaService cuentaService)
		{
			_cuentaService = cuentaService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(
				await _cuentaService.GetAllAsync());
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			return Ok(
				await _cuentaService.GetByIdAsync(id));
		}

		[HttpPost]
		public async Task<IActionResult> Create(
			CreateCuentaDto dto)
		{
			var cuenta =
				await _cuentaService.CreateAsync(dto);

			return CreatedAtAction(
				nameof(GetById),
				new { id = cuenta.Id },
				cuenta);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update(	int id, UpdateCuentaDto dto)
		{
			return Ok(
				await _cuentaService.UpdateAsync(
					id,
					dto));
		}
	}
}