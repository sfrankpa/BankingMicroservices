namespace AccountService.Application.DTOs
{
	public class UpdateMovimientoDto
	{
		public string TipoMovimiento { get; set; } = string.Empty;

		public decimal Valor { get; set; }
	}
}