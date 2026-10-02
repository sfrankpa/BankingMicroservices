namespace Shared.Contracts.Events
{
	public class ClienteCreadoEvent
	{
		public int ClienteId { get; set; }

		public string Nombre { get; set; } = string.Empty;

		public string Identificacion { get; set; } = string.Empty;

		public bool Estado { get; set; }
	}
}