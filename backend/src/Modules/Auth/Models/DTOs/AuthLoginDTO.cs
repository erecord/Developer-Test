using System.ComponentModel.DataAnnotations;

namespace StoreBackend.DTOs
{
    public class AuthLoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}