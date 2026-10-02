namespace AccountService.Application.DTOs
{
	public class CuentaDto
	{
		public int Id { get; set; }

		public string NumeroCuenta { get; set; } = string.Empty;

		public string TipoCuenta { get; set; } = string.Empty;

		public decimal SaldoInicial { get; set; }

		public decimal SaldoActual { get; set; }

		public bool Estado { get; set; }

		public int ClienteId { get; set; }
	}
}