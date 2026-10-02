using CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Data
{
	public class CustomerDbContext : DbContext
	{
		public CustomerDbContext(
			DbContextOptions<CustomerDbContext> options)
			: base(options)
		{
		}

		public DbSet<Persona> Personas { get; set; }
		public DbSet<Cliente> Clientes { get; set; }

		protected override void OnModelCreating(
			ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Persona>(entity =>
			{
				entity.HasKey(p => p.Id);

				entity.Property(p => p.Nombre)
					.IsRequired()
					.HasMaxLength(150);

				entity.Property(p => p.Genero)
					.HasMaxLength(20);

				entity.Property(p => p.Identificacion)
					.IsRequired()
					.HasMaxLength(20);

				entity.HasIndex(p => p.Identificacion)
					.IsUnique();

				entity.Property(p => p.Direccion)
					.HasMaxLength(250);

				entity.Property(p => p.Telefono)
					.HasMaxLength(20);
			});

			modelBuilder.Entity<Cliente>(entity =>
			{
				entity.Property(c => c.Contrasena)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(c => c.Estado)
					.IsRequired();
			});
		}
	}
}