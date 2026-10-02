namespace CustomerService.Domain.Entities
{
	public class Cliente : Persona
	{
		public string Contrasena { get; set; } = string.Empty;

		public bool Estado { get; set; }
	}
}