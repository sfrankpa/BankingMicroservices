using AccountService.Domain.Entities;

namespace AccountService.Application.Interfaces
{
	public interface ICuentaRepository
	{
		Task<List<Cuenta>> GetAllAsync();

		Task<Cuenta?> GetByIdAsync(int id);

		Task<Cuenta?> GetByNumeroCuentaAsync(
			string numeroCuenta);

		Task<List<Cuenta>> GetByClienteIdAsync(
			int clienteId);

		Task AddAsync(Cuenta cuenta);

		void Update(Cuenta cuenta);

		Task SaveChangesAsync();
	}
}