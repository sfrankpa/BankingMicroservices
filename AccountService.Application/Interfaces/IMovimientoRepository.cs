using AccountService.Domain.Entities;

namespace AccountService.Application.Interfaces
{
	public interface IMovimientoRepository
	{
		Task<List<Movimiento>> GetAllAsync();

		Task<Movimiento?> GetByIdAsync(int id);

		Task<List<Movimiento>> GetByCuentaIdAsync(
			int cuentaId);

		Task<List<Movimiento>> GetByCuentaAndDateRangeAsync(
			int cuentaId,
			DateTime desde,
			DateTime hasta);

		Task AddAsync(Movimiento movimiento);

		void Update(Movimiento movimiento);

		Task SaveChangesAsync();
	}
}