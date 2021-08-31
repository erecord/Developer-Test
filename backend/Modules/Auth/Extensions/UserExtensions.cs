using StoreBackend.Models;

namespace StoreBackend.Auth.Extensions
{
    public static class UserExtenions
    {
        public static UserStrippedDTO ToUserStrippedDTO(this User user) =>
        new UserStrippedDTO { Id = user.Id, Username = user.Username, Email = user.Email };
    }
}