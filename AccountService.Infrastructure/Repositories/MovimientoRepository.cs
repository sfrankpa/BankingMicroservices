using AccountService.Application.Interfaces;
using AccountService.Domain.Entities;
using AccountService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Repositories
{
	public class MovimientoRepository : IMovimientoRepository
	{
		private readonly AccountDbContext _context;

		public MovimientoRepository(AccountDbContext context)
		{
			_context = context;
		}

		public async Task<List<Movimiento>> GetAllAsync()
		{
			return await _context.Movimientos
				.AsNoTracking()
				.Include(m => m.Cuenta)
				.ToListAsync();
		}

		public async Task<Movimiento?> GetByIdAsync(int id)
		{
			return await _context.Movimientos
				.Include(m => m.Cuenta)
				.FirstOrDefaultAsync(m => m.Id == id);
		}

		public async Task<List<Movimiento>> GetByCuentaIdAsync(
			int cuentaId)
		{
			return await _context.Movimientos
				.AsNoTracking()
				.Where(m => m.CuentaId == cuentaId)
				.OrderByDescending(m => m.Fecha)
				.ToListAsync();
		}

		public async Task<List<Movimiento>>
			GetByCuentaAndDateRangeAsync(
				int cuentaId,
				DateTime desde,
				DateTime hasta)
		{
			return await _context.Movimientos
				.AsNoTracking()
				.Where(m =>
					m.CuentaId == cuentaId &&
					m.Fecha >= desde &&
					m.Fecha <= hasta)
				.OrderBy(m => m.Fecha)
				.ToListAsync();
		}

		public async Task AddAsync(Movimiento movimiento)
		{
			await _context.Movimientos.AddAsync(movimiento);
		}

		public void Update(Movimiento movimiento)
		{
			_context.Movimientos.Update(movimiento);
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}