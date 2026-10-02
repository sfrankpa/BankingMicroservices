namespace AccountService.Application.DTOs
{
	public class ReporteCuentaDto
	{
		public int CuentaId { get; set; }

		public string NumeroCuenta { get; set; } = string.Empty;

		public string TipoCuenta { get; set; } = string.Empty;

		public decimal SaldoInicial { get; set; }

		public decimal SaldoActual { get; set; }

		public bool Estado { get; set; }

		public List<ReporteMovimientoDto> Movimientos { get; set; }
			= new();
	}
}