using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StoreBackend.Auth.Interfaces;
using StoreBackend.Models;

namespace StoreBackend.Auth.Services
{

    public class JwtService : IJwtService
    {

        public string GetJwtToken(User user)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(getSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static void RegisterService(IServiceCollection servies)
        {
            servies.AddSingleton<IJwtService>(new JwtService());

            servies.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = getSymmetricSecurityKey(),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        private static string getJwtKey() => Environment.GetEnvironmentVariable("JwtKey");
        private static SymmetricSecurityKey getSymmetricSecurityKey()
        {
            var jwtKeyAsByteArray = Encoding.ASCII.GetBytes(getJwtKey());
            return new SymmetricSecurityKey(jwtKeyAsByteArray);
        }
    }
}