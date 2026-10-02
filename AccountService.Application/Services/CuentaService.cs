using AccountService.Application.DTOs;
using AccountService.Application.Exceptions;
using AccountService.Application.Interfaces;
using AccountService.Domain.Entities;

namespace AccountService.Application.Services
{
	public class CuentaService : ICuentaService
	{
		private readonly ICuentaRepository _cuentaRepository;
		private readonly IClienteReferenciaRepository _clienteRepository;

		public CuentaService(
			ICuentaRepository cuentaRepository,
			IClienteReferenciaRepository clienteRepository)
		{
			_cuentaRepository = cuentaRepository;
			_clienteRepository = clienteRepository;
		}

		public async Task<List<CuentaDto>> GetAllAsync()
		{
			var cuentas = await _cuentaRepository.GetAllAsync();

			return cuentas
				.Select(MapToDto)
				.ToList();
		}

		public async Task<CuentaDto> GetByIdAsync(int id)
		{
			var cuenta = await _cuentaRepository.GetByIdAsync(id);

			if (cuenta == null)
			{
				throw new NotFoundException(
					"Cuenta no encontrada.");
			}

			return MapToDto(cuenta);
		}

		public async Task<CuentaDto> CreateAsync(	CreateCuentaDto dto)
		{
			if (string.IsNullOrWhiteSpace(dto.NumeroCuenta))
			{
				throw new BusinessException(
					"El número de cuenta es obligatorio.");
			}

			if (string.IsNullOrWhiteSpace(dto.TipoCuenta))
			{
				throw new BusinessException(
					"El tipo de cuenta es obligatorio.");
			}

			if (dto.SaldoInicial < 0)
			{
				throw new BusinessException(
					"El saldo inicial no puede ser negativo.");
			}

			var cliente = await _clienteRepository.GetByClienteIdAsync(dto.ClienteId);

			if (cliente == null)
			{
				throw new BusinessException(
					"El cliente no existe.");
			}

			if (!cliente.Estado)
			{
				throw new BusinessException(
					"El cliente está inactivo.");
			}

			var existente = await _cuentaRepository	.GetByNumeroCuentaAsync(dto.NumeroCuenta);

			if (existente != null)
			{
				throw new BusinessException(
					"Ya existe una cuenta con ese número.");
			}

			var cuenta = new Cuenta
			{
				NumeroCuenta = dto.NumeroCuenta,
				TipoCuenta = dto.TipoCuenta,
				SaldoInicial = dto.SaldoInicial,
				SaldoActual = dto.SaldoInicial,
				Estado = dto.Estado,
				ClienteId = dto.ClienteId
			};

			await _cuentaRepository.AddAsync(cuenta);
			await _cuentaRepository.SaveChangesAsync();

			return MapToDto(cuenta);
		}

		public async Task<CuentaDto> UpdateAsync(int id, UpdateCuentaDto dto)
		{
			var cuenta = await _cuentaRepository.GetByIdAsync(id);

			if (cuenta == null)
			{
				throw new NotFoundException(
					"Cuenta no encontrada.");
			}

			if (string.IsNullOrWhiteSpace(dto.TipoCuenta))
			{
				throw new BusinessException(
					"El tipo de cuenta es obligatorio.");
			}

			cuenta.TipoCuenta = dto.TipoCuenta;
			cuenta.Estado = dto.Estado;

			_cuentaRepository.Update(cuenta);

			await _cuentaRepository.SaveChangesAsync();

			return MapToDto(cuenta);
		}

		private static CuentaDto MapToDto(Cuenta cuenta)
		{
			return new CuentaDto
			{
				Id = cuenta.Id,
				NumeroCuenta = cuenta.NumeroCuenta,
				TipoCuenta = cuenta.TipoCuenta,
				SaldoInicial = cuenta.SaldoInicial,
				SaldoActual = cuenta.SaldoActual,
				Estado = cuenta.Estado,
				ClienteId = cuenta.ClienteId
			};
		}
	}
}