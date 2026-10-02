using AccountService.Application.Interfaces;
using AccountService.Domain.Entities;
using AccountService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Repositories
{
	public class ClienteReferenciaRepository
		: IClienteReferenciaRepository
	{
		private readonly AccountDbContext _context;

		public ClienteReferenciaRepository(
			AccountDbContext context)
		{
			_context = context;
		}

		public async Task<ClienteReferencia?>
			GetByClienteIdAsync(int clienteId)
		{
			return await _context.ClientesReferencia
				.FirstOrDefaultAsync(
					c => c.ClienteId == clienteId);
		}

		public async Task AddAsync(
			ClienteReferencia cliente)
		{
			await _context.ClientesReferencia
				.AddAsync(cliente);
		}

		public void Update(
			ClienteReferencia cliente)
		{
			_context.ClientesReferencia.Update(cliente);
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}