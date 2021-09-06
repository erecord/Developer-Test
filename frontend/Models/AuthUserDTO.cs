namespace Store.Frontend.Models
{
    public class AuthUserDTO
    {
        public AuthUserDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}