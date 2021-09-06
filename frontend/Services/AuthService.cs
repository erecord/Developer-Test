using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Store.Frontend.Models;

namespace Store.Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> Authenticate(string email, string password)
        {
            var user = new AuthUserDTO { Email = email, Password = password };
            var dataAsString = JsonConvert.SerializeObject(user);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await _http.PostAsync("auth/authenticate", content);
            var jwt = await result.Content.ReadAsStringAsync();
            return jwt;
        }
    }
}