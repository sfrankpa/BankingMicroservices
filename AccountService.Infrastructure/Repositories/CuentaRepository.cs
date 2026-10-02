using AccountService.Application.Interfaces;
using AccountService.Domain.Entities;
using AccountService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Repositories
{
	public class CuentaRepository : ICuentaRepository
	{
		private readonly AccountDbContext _context;

		public CuentaRepository(AccountDbContext context)
		{
			_context = context;
		}

		public async Task<List<Cuenta>> GetAllAsync()
		{
			return await _context.Cuentas
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<Cuenta?> GetByIdAsync(int id)
		{
			return await _context.Cuentas
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<Cuenta?> GetByNumeroCuentaAsync(
			string numeroCuenta)
		{
			return await _context.Cuentas
				.FirstOrDefaultAsync(
					c => c.NumeroCuenta == numeroCuenta);
		}

		public async Task<List<Cuenta>> GetByClienteIdAsync(
			int clienteId)
		{
			return await _context.Cuentas
				.AsNoTracking()
				.Where(c => c.ClienteId == clienteId)
				.ToListAsync();
		}

		public async Task AddAsync(Cuenta cuenta)
		{
			await _context.Cuentas.AddAsync(cuenta);
		}

		public void Update(Cuenta cuenta)
		{
			_context.Cuentas.Update(cuenta);
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}