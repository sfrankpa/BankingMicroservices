namespace AccountService.Application.DTOs
{
	public class CreateMovimientoDto
	{
		public int CuentaId { get; set; }

		public string TipoMovimiento { get; set; } = string.Empty;

		public decimal Valor { get; set; }
	}
}