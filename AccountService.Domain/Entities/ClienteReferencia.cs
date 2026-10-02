namespace AccountService.Domain.Entities
{
	public class ClienteReferencia
	{
		public int Id { get; set; }

		public int ClienteId { get; set; }

		public string Nombre { get; set; } = string.Empty;

		public string Identificacion { get; set; } = string.Empty;

		public bool Estado { get; set; }
	}
}