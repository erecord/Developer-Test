using StoreBackend.DTOs;
using StoreBackend.Models;

namespace StoreBackend.Auth.Extensions
{
    public static class UserExtenions
    {
        public static UserDTO ToDTO(this User user) =>
        new UserDTO { Id = user.Id, Username = user.Username, Email = user.Email };
    }
}