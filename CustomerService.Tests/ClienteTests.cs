using CustomerService.Domain.Entities;

namespace CustomerService.Tests
{
	public class ClienteTests
	{
		[Fact]
		public void CrearCliente_DebeAsignarCorrectamenteSusPropiedades()
		{
			// Arrange
			var cliente = new Cliente
			{
				Id = 1,
				Nombre = "Jose Lema",
				Genero = "Masculino",
				Edad = 35,
				Identificacion = "1100000001",
				Direccion = "Otavalo sn y principal",
				Telefono = "098254785",
				Contrasena = "1234",
				Estado = true
			};

			// Act
			var clienteId = cliente.Id;
			var nombre = cliente.Nombre;
			var identificacion = cliente.Identificacion;
			var estado = cliente.Estado;

			// Assert
			Assert.Equal(1, clienteId);
			Assert.Equal("Jose Lema", nombre);
			Assert.Equal("1100000001", identificacion);
			Assert.True(estado);
		}
	}
}