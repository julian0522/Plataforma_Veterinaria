namespace Veterinaria.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string  Email { get; set; }
        public string EmailNormalizado { get; set; }
        public string PasswordHash { get; set; }
        public string Nombre { get; set; }
        public int Cedula { get; set; }
        public string Telefono { get; set; }
        public int Edad { get; set; }
    }
}
