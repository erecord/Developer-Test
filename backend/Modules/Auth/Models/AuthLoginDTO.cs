using System.ComponentModel.DataAnnotations;

namespace StoreBackend.Models
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