namespace AccountService.Application.DTOs
{
	public class ReporteMovimientoDto
	{
		public DateTime Fecha { get; set; }

		public string TipoMovimiento { get; set; } = string.Empty;

		public decimal Valor { get; set; }

		public decimal Saldo { get; set; }
	}
}