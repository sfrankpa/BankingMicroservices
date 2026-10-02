using AccountService.Application.DTOs;
using AccountService.Application.Exceptions;
using AccountService.Application.Interfaces;

namespace AccountService.Application.Services
{
	public class ReporteService : IReporteService
	{
		private readonly IClienteReferenciaRepository _clienteRepository;
		private readonly ICuentaRepository _cuentaRepository;
		private readonly IMovimientoRepository _movimientoRepository;

		public ReporteService(
			IClienteReferenciaRepository clienteRepository,
			ICuentaRepository cuentaRepository,
			IMovimientoRepository movimientoRepository)
		{
			_clienteRepository = clienteRepository;
			_cuentaRepository = cuentaRepository;
			_movimientoRepository = movimientoRepository;
		}

		public async Task<ReporteClienteDto> GetEstadoCuentaAsync(
			int clienteId,
			DateTime desde,
			DateTime hasta)
		{
			if (desde > hasta)
			{
				throw new BusinessException(
					"La fecha desde no puede ser mayor que la fecha hasta.");
			}

			var cliente =
				await _clienteRepository
					.GetByClienteIdAsync(clienteId);

			if (cliente == null)
			{
				throw new NotFoundException(
					"Cliente no encontrado.");
			}

			var cuentas =
				await _cuentaRepository
					.GetByClienteIdAsync(clienteId);

			var hastaFinDia = hasta.Date
				.AddDays(1)
				.AddTicks(-1);

			var resultado = new ReporteClienteDto
			{
				ClienteId = cliente.ClienteId,
				Nombre = cliente.Nombre,
				Identificacion = cliente.Identificacion,
				Desde = desde,
				Hasta = hasta
			};

			foreach (var cuenta in cuentas)
			{
				var movimientos =
					await _movimientoRepository
						.GetByCuentaAndDateRangeAsync(
							cuenta.Id,
							desde.Date,
							hastaFinDia);

				resultado.Cuentas.Add(
					new ReporteCuentaDto
					{
						CuentaId = cuenta.Id,
						NumeroCuenta = cuenta.NumeroCuenta,
						TipoCuenta = cuenta.TipoCuenta,
						SaldoInicial = cuenta.SaldoInicial,
						SaldoActual = cuenta.SaldoActual,
						Estado = cuenta.Estado,

						Movimientos = movimientos
							.Select(m =>
								new ReporteMovimientoDto
								{
									Fecha = m.Fecha,
									TipoMovimiento =
										m.TipoMovimiento,
									Valor = m.Valor,
									Saldo = m.Saldo
								})
							.ToList()
					});
			}

			return resultado;
		}
	}
}