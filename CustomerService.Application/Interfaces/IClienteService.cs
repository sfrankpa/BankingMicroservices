using CustomerService.Application.DTOs;

namespace CustomerService.Application.Interfaces
{
	public interface IClienteService
	{
		Task<List<ClienteDto>> GetAllAsync();

		Task<ClienteDto> GetByIdAsync(int id);

		Task<ClienteDto> CreateAsync(
			CreateClienteDto dto);

		Task<ClienteDto> UpdateAsync(
			int id,
			UpdateClienteDto dto);

		Task DeleteAsync(int id);
	}
}