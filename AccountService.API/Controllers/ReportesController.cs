using AccountService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.API.Controllers
{
	[ApiController]
	[Route("api/reportes")]
	public class ReportesController : ControllerBase
	{
		private readonly IReporteService _reporteService;

		public ReportesController(
			IReporteService reporteService)
		{
			_reporteService = reporteService;
		}

		[HttpGet]
		public async Task<IActionResult> GetEstadoCuenta(
			[FromQuery] int clienteId,
			[FromQuery] DateTime desde,
			[FromQuery] DateTime hasta)
		{
			var reporte =
				await _reporteService
					.GetEstadoCuentaAsync(
						clienteId,
						desde,
						hasta);

			return Ok(reporte);
		}
	}
}