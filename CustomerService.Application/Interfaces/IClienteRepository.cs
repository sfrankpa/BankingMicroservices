using CustomerService.Domain.Entities;

namespace CustomerService.Application.Interfaces
{
	public interface IClienteRepository
	{
		Task<List<Cliente>> GetAllAsync();

		Task<Cliente?> GetByIdAsync(int id);

		Task<Cliente?> GetByIdentificacionAsync(string identificacion);

		Task AddAsync(Cliente cliente);

		void Update(Cliente cliente);

		void Delete(Cliente cliente);

		Task SaveChangesAsync();
	}
}