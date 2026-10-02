using AccountService.Domain.Entities;

namespace AccountService.Application.Interfaces
{
	public interface IClienteReferenciaRepository
	{
		Task<ClienteReferencia?> GetByClienteIdAsync(
			int clienteId);

		Task AddAsync(ClienteReferencia cliente);

		void Update(ClienteReferencia cliente);

		Task SaveChangesAsync();
	}
}