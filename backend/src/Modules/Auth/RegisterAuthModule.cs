using Microsoft.Extensions.DependencyInjection;
using StoreBackend.Auth.Interfaces;
using StoreBackend.Auth.Services;

namespace StoreBackend.Modules
{
    public class RegisterAuthModule
    {
        public RegisterAuthModule(IServiceCollection services)
        {
            var jwtService = new JwtService();
            jwtService.RegisterAuthenticationService(services);
            services.AddSingleton<IJwtService>(jwtService);
        }
    }
}