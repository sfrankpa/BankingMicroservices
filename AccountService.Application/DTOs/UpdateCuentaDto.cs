namespace AccountService.Application.DTOs
{
	public class UpdateCuentaDto
	{
		public string TipoCuenta { get; set; } = string.Empty;

		public bool Estado { get; set; }
	}
}