using StoreBackend.Models;

namespace StoreBackend.Auth.Interfaces
{

    public interface IJwtService
    {
        string GetJwtToken(User user);
    }
}