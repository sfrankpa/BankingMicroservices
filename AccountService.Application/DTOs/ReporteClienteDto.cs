namespace AccountService.Application.DTOs
{
	public class ReporteClienteDto
	{
		public int ClienteId { get; set; }

		public string Nombre { get; set; } = string.Empty;

		public string Identificacion { get; set; } = string.Empty;

		public DateTime Desde { get; set; }

		public DateTime Hasta { get; set; }

		public List<ReporteCuentaDto> Cuentas { get; set; }
			= new();
	}
}