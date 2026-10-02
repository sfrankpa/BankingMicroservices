using AccountService.Application.DTOs;
using AccountService.Application.Exceptions;
using AccountService.Application.Interfaces;
using AccountService.Domain.Entities;

namespace AccountService.Application.Services
{
	public class MovimientoService : IMovimientoService
	{
		private readonly IMovimientoRepository _movimientoRepository;
		private readonly ICuentaRepository _cuentaRepository;

		public MovimientoService(
			IMovimientoRepository movimientoRepository,
			ICuentaRepository cuentaRepository)
		{
			_movimientoRepository = movimientoRepository;
			_cuentaRepository = cuentaRepository;
		}

		public async Task<List<MovimientoDto>> GetAllAsync()
		{
			var movimientos = 	await _movimientoRepository.GetAllAsync();

			return movimientos
				.Select(MapToDto)
				.ToList();
		}

		public async Task<MovimientoDto> GetByIdAsync(int id)
		{
			var movimiento = await _movimientoRepository.GetByIdAsync(id);

			if (movimiento == null)
			{
				throw new NotFoundException("Movimiento no encontrado.");
			}

			return MapToDto(movimiento);
		}

		public async Task<List<MovimientoDto>> GetByCuentaIdAsync(	int cuentaId)
		{
			var cuenta = await _cuentaRepository.GetByIdAsync(cuentaId);

			if (cuenta == null)
			{
				throw new NotFoundException(	"Cuenta no encontrada.");
			}

			var movimientos = 	await _movimientoRepository	.GetByCuentaIdAsync(cuentaId);

			return movimientos
				.Select(MapToDto)
				.ToList();
		}

		public async Task<MovimientoDto> CreateAsync(	CreateMovimientoDto dto)
		{
			var cuenta = await _cuentaRepository.GetByIdAsync(dto.CuentaId);

			if (cuenta == null)
			{
				throw new NotFoundException(	"Cuenta no encontrada.");
			}

			if (!cuenta.Estado)
			{
				throw new BusinessException("La cuenta se encuentra inactiva.");
			}

			if (string.IsNullOrWhiteSpace(	dto.TipoMovimiento))
			{
				throw new BusinessException("El tipo de movimiento es obligatorio.");
			}

			if (dto.Valor == 0)
			{
				throw new BusinessException("El valor del movimiento no puede ser cero.");
			}

			var nuevoSaldo = cuenta.SaldoActual + dto.Valor;

			if (nuevoSaldo < 0)
			{
				throw new BusinessException("Saldo no disponible");
			}

			var movimiento = new Movimiento
			{
				Fecha = DateTime.UtcNow,
				TipoMovimiento = dto.TipoMovimiento,
				Valor = dto.Valor,
				Saldo = nuevoSaldo,
				CuentaId = cuenta.Id
			};

			cuenta.SaldoActual = nuevoSaldo;

			await _movimientoRepository	.AddAsync(movimiento);

			_cuentaRepository.Update(cuenta);

			await _movimientoRepository	.SaveChangesAsync();

			return MapToDto(movimiento);
		}

		public async Task<MovimientoDto> UpdateAsync(int id, UpdateMovimientoDto dto)
		{
			var movimiento = await _movimientoRepository.GetByIdAsync(id);

			if (movimiento == null)
			{
				throw new NotFoundException(	"Movimiento no encontrado.");
			}

			if (string.IsNullOrWhiteSpace(	dto.TipoMovimiento))
			{
				throw new BusinessException("El tipo de movimiento es obligatorio.");
			}

			if (dto.Valor == 0)
			{
				throw new BusinessException("El valor del movimiento no puede ser cero.");
			}

			var cuenta = await _cuentaRepository.GetByIdAsync(movimiento.CuentaId);

			if (cuenta == null)
			{
				throw new NotFoundException("Cuenta no encontrada.");
			}

			if (!cuenta.Estado)
			{
				throw new BusinessException("La cuenta se encuentra inactiva.");
			}

			// Revertimos el valor anterior
			var saldoSinMovimientoAnterior = cuenta.SaldoActual - movimiento.Valor;

			// Aplicamos el nuevo valor
			var nuevoSaldo = saldoSinMovimientoAnterior + dto.Valor;

			if (nuevoSaldo < 0)
			{
				throw new BusinessException(
					"Saldo no disponible");
			}

			movimiento.TipoMovimiento =	dto.TipoMovimiento;
			movimiento.Valor =	 dto.Valor;
			movimiento.Saldo = nuevoSaldo;
			cuenta.SaldoActual = nuevoSaldo;

			_movimientoRepository.Update(	movimiento);

			_cuentaRepository.Update(	cuenta);

			await _movimientoRepository.SaveChangesAsync();

			return MapToDto(movimiento);
		}

		private static MovimientoDto MapToDto(Movimiento movimiento)
		{
			return new MovimientoDto
			{
				Id = movimiento.Id,
				Fecha = movimiento.Fecha,
				TipoMovimiento =	movimiento.TipoMovimiento,
				Valor = movimiento.Valor,
				Saldo = movimiento.Saldo,
				CuentaId = movimiento.CuentaId
			};
		}
	}
}