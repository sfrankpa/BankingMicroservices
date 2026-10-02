using AccountService.Application.DTOs;
using AccountService.Application.Exceptions;
using AccountService.Application.Interfaces;
using AccountService.Application.Services;
using AccountService.Domain.Entities;
using Moq;

namespace AccountService.Tests
{
	public class MovimientoServiceTests
	{
		[Fact]
		public async Task MovimientoSinSaldoSuficiente_DebeLanzarSaldoNoDisponible()
		{
			// Arrange
			var movimientoRepositoryMock =
				new Mock<IMovimientoRepository>();

			var cuentaRepositoryMock =
				new Mock<ICuentaRepository>();

			var cuenta = new Cuenta
			{
				Id = 1,
				NumeroCuenta = "478758",
				TipoCuenta = "Ahorros",
				SaldoInicial = 100,
				SaldoActual = 100,
				Estado = true,
				ClienteId = 1
			};

			cuentaRepositoryMock
				.Setup(r => r.GetByIdAsync(1))
				.ReturnsAsync(cuenta);

			var service = new MovimientoService(
				movimientoRepositoryMock.Object,
				cuentaRepositoryMock.Object);

			var dto = new CreateMovimientoDto
			{
				CuentaId = 1,
				TipoMovimiento = "Retiro",
				Valor = -500
			};

			// Act
			var exception =
				await Assert.ThrowsAsync<BusinessException>(
					() => service.CreateAsync(dto));

			// Assert
			Assert.Equal(
				"Saldo no disponible",
				exception.Message);

			Assert.Equal(
				100,
				cuenta.SaldoActual);

			movimientoRepositoryMock.Verify(
				r => r.AddAsync(
					It.IsAny<Movimiento>()),
				Times.Never);

			movimientoRepositoryMock.Verify(
				r => r.SaveChangesAsync(),
				Times.Never);
		}

		[Fact]
		public async Task DepositoValido_DebeActualizarSaldoYGuardarMovimiento()
		{
			// Arrange
			var movimientoRepositoryMock =
				new Mock<IMovimientoRepository>();

			var cuentaRepositoryMock =
				new Mock<ICuentaRepository>();

			var cuenta = new Cuenta
			{
				Id = 1,
				NumeroCuenta = "478758",
				TipoCuenta = "Ahorros",
				SaldoInicial = 100,
				SaldoActual = 100,
				Estado = true,
				ClienteId = 1
			};

			cuentaRepositoryMock
				.Setup(r => r.GetByIdAsync(1))
				.ReturnsAsync(cuenta);

			movimientoRepositoryMock
				.Setup(r => r.AddAsync(
					It.IsAny<Movimiento>()))
				.Returns(Task.CompletedTask);

			movimientoRepositoryMock
				.Setup(r => r.SaveChangesAsync())
				.Returns(Task.CompletedTask);

			var service = new MovimientoService(
				movimientoRepositoryMock.Object,
				cuentaRepositoryMock.Object);

			var dto = new CreateMovimientoDto
			{
				CuentaId = 1,
				TipoMovimiento = "Deposito",
				Valor = 600
			};

			// Act
			var resultado =
				await service.CreateAsync(dto);

			// Assert
			Assert.Equal(700, cuenta.SaldoActual);

			Assert.Equal(600, resultado.Valor);

			Assert.Equal(700, resultado.Saldo);

			Assert.Equal("Deposito", resultado.TipoMovimiento);

			Assert.Equal(1, resultado.CuentaId);

			movimientoRepositoryMock.Verify(
				r => r.AddAsync(
					It.Is<Movimiento>(
						m =>
							m.CuentaId == 1 &&
							m.Valor == 600 &&
							m.Saldo == 700)),
				Times.Once);

			cuentaRepositoryMock.Verify(
				r => r.Update(cuenta),
				Times.Once);

			movimientoRepositoryMock.Verify(
				r => r.SaveChangesAsync(),
				Times.Once);
		}
	}
}