using AccountService.Application.DTOs;

namespace AccountService.Application.Interfaces
{
	public interface IMovimientoService
	{
		Task<List<MovimientoDto>> GetAllAsync();

		Task<MovimientoDto> GetByIdAsync(int id);

		Task<List<MovimientoDto>> GetByCuentaIdAsync(	int cuentaId);

		Task<MovimientoDto> CreateAsync(	CreateMovimientoDto dto);

		Task<MovimientoDto> UpdateAsync(int id, UpdateMovimientoDto dto);
	}
}