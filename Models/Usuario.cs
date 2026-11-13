namespace ProjetoMoveisMedeiros.Api.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegistroDto
    {
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }   
}
