using AccountService.Application.DTOs;
using AccountService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.API.Controllers
{
	[ApiController]
	[Route("api/movimientos")]
	public class MovimientosController : ControllerBase
	{
		private readonly IMovimientoService _movimientoService;

		public MovimientosController(
			IMovimientoService movimientoService)
		{
			_movimientoService = movimientoService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(
				await _movimientoService.GetAllAsync());
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			return Ok(
				await _movimientoService
					.GetByIdAsync(id));
		}

		[HttpGet("cuenta/{cuentaId:int}")]
		public async Task<IActionResult> GetByCuenta(
			int cuentaId)
		{
			return Ok(
				await _movimientoService
					.GetByCuentaIdAsync(cuentaId));
		}

		[HttpPost]
		public async Task<IActionResult> Create(
			CreateMovimientoDto dto)
		{
			var movimiento =
				await _movimientoService
					.CreateAsync(dto);

			return CreatedAtAction(
				nameof(GetById),
				new { id = movimiento.Id },
				movimiento);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update(
			int id,
			UpdateMovimientoDto dto)
		{
			return Ok(
				await _movimientoService
					.UpdateAsync(id, dto));
		}
	}
}