using CustomerService.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using ApiProgram = global::Program;

namespace CustomerService.Tests
{
	public class CustomWebApplicationFactory
		: WebApplicationFactory<ApiProgram>
	{
		protected override void ConfigureWebHost(
			IWebHostBuilder builder)
		{
			builder.UseEnvironment("Testing");

			builder.ConfigureServices(services =>
			{
				var descriptor =
					services.SingleOrDefault(
						d => d.ServiceType ==
							 typeof(
								 DbContextOptions<
									 CustomerDbContext>));

				if (descriptor != null)
				{
					services.Remove(descriptor);
				}

				services.AddDbContext<CustomerDbContext>(
					options =>
					{
						options.UseInMemoryDatabase(
							"CustomerTestDb");
					});
			});
		}
	}
}