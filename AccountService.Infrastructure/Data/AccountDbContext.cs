using AccountService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Data
{
	public class AccountDbContext : DbContext
	{
		public AccountDbContext(
			DbContextOptions<AccountDbContext> options)
			: base(options)
		{
		}

		public DbSet<Cuenta> Cuentas { get; set; }

		public DbSet<Movimiento> Movimientos { get; set; }

		public DbSet<ClienteReferencia> ClientesReferencia
		{ get; set; }

		protected override void OnModelCreating(
			ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ClienteReferencia>(entity =>
			{
				entity.HasKey(c => c.Id);

				entity.HasIndex(c => c.ClienteId)
					.IsUnique();

				entity.Property(c => c.Nombre)
					.IsRequired()
					.HasMaxLength(150);

				entity.Property(c => c.Identificacion)
					.IsRequired()
					.HasMaxLength(20);

				entity.HasIndex(c => c.Identificacion)
					.IsUnique();
			});

			modelBuilder.Entity<Cuenta>(entity =>
			{
				entity.HasKey(c => c.Id);

				entity.Property(c => c.NumeroCuenta)
					.IsRequired()
					.HasMaxLength(20);

				entity.HasIndex(c => c.NumeroCuenta)
					.IsUnique();

				entity.Property(c => c.TipoCuenta)
					.IsRequired()
					.HasMaxLength(20);

				entity.Property(c => c.SaldoInicial)
					.HasColumnType("decimal(18,2)");

				entity.Property(c => c.SaldoActual)
					.HasColumnType("decimal(18,2)");
			});

			modelBuilder.Entity<Movimiento>(entity =>
			{
				entity.HasKey(m => m.Id);

				entity.Property(m => m.TipoMovimiento)
					.IsRequired()
					.HasMaxLength(30);

				entity.Property(m => m.Valor)
					.HasColumnType("decimal(18,2)");

				entity.Property(m => m.Saldo)
					.HasColumnType("decimal(18,2)");

				entity.HasOne(m => m.Cuenta)
					.WithMany(c => c.Movimientos)
					.HasForeignKey(m => m.CuentaId)
					.OnDelete(DeleteBehavior.Cascade);
			});
		}
	}
}