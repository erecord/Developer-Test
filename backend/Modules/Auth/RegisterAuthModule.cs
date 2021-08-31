using Microsoft.Extensions.DependencyInjection;
using StoreBackend.Auth.Interfaces;
using StoreBackend.Auth.Services;

namespace StoreBackend.Modules
{
    public class RegisterAuthModule
    {
        public RegisterAuthModule(IServiceCollection services)
        {
            services.AddSingleton<IJwtService>(sp =>
            {
                var jwtService = new JwtService();
                jwtService.RegisterAuthenticationService(services);

                return jwtService;
            });
        }
    }
}