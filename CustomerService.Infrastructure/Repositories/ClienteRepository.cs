using CustomerService.Application.Interfaces;
using CustomerService.Domain.Entities;
using CustomerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Repositories
{
	public class ClienteRepository : IClienteRepository
	{
		private readonly CustomerDbContext _context;

		public ClienteRepository(CustomerDbContext context)
		{
			_context = context;
		}

		public async Task<List<Cliente>> GetAllAsync()
		{
			return await _context.Clientes
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<Cliente?> GetByIdAsync(int id)
		{
			return await _context.Clientes
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<Cliente?> GetByIdentificacionAsync(
			string identificacion)
		{
			return await _context.Clientes
				.FirstOrDefaultAsync(
					c => c.Identificacion == identificacion);
		}

		public async Task AddAsync(Cliente cliente)
		{
			await _context.Clientes.AddAsync(cliente);
		}

		public void Update(Cliente cliente)
		{
			_context.Clientes.Update(cliente);
		}

		public void Delete(Cliente cliente)
		{
			_context.Clientes.Remove(cliente);
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}