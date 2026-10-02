using AccountService.Application.DTOs;

namespace AccountService.Application.Interfaces
{
	public interface IReporteService
	{
		Task<ReporteClienteDto> GetEstadoCuentaAsync(
			int clienteId,
			DateTime desde,
			DateTime hasta);
	}
}