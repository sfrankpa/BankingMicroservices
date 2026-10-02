namespace AccountService.Domain.Entities
{
	public class Cuenta
	{
		public int Id { get; set; }

		public string NumeroCuenta { get; set; } = string.Empty;

		public string TipoCuenta { get; set; } = string.Empty;

		public decimal SaldoInicial { get; set; }

		public decimal SaldoActual { get; set; }

		public bool Estado { get; set; }

		public int ClienteId { get; set; }

		public ICollection<Movimiento> Movimientos { get; set; }
			= new List<Movimiento>();
	}
}