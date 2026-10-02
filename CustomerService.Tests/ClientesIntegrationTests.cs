using System.Net;
using ApiProgram = global::Program;

namespace CustomerService.Tests
{
	public class ClientesIntegrationTests
		: IClassFixture<CustomWebApplicationFactory>
	{
		private readonly HttpClient _client;

		public ClientesIntegrationTests(
			CustomWebApplicationFactory factory)
		{
			_client = factory.CreateClient();
		}

		[Fact]
		public async Task GetClientes_DebeResponderCorrectamente()
		{
			// Act
			var response =
				await _client.GetAsync("/api/clientes");

			// Assert
			Assert.Equal(
				HttpStatusCode.OK,
				response.StatusCode);
		}
	}
}