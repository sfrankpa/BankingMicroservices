using AccountService.Application.DTOs;

namespace AccountService.Application.Interfaces
{
	public interface ICuentaService
	{
		Task<List<CuentaDto>> GetAllAsync();

		Task<CuentaDto> GetByIdAsync(int id);

		Task<CuentaDto> CreateAsync(
			CreateCuentaDto dto);

		Task<CuentaDto> UpdateAsync(
			int id,
			UpdateCuentaDto dto);
	}
}