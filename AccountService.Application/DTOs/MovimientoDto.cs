namespace AccountService.Application.DTOs
{
	public class MovimientoDto
	{
		public int Id { get; set; }

		public DateTime Fecha { get; set; }

		public string TipoMovimiento { get; set; } = string.Empty;

		public decimal Valor { get; set; }

		public decimal Saldo { get; set; }

		public int CuentaId { get; set; }
	}
}