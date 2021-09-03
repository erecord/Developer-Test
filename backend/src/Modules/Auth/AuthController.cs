using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoreBackend.Auth.Extensions;
using StoreBackend.Auth.Interfaces;
using StoreBackend.DbContexts;
using StoreBackend.Models;
using BC = BCrypt.Net.BCrypt;

namespace StoreBackend.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;
        private StoreDbContext _context;

        public AuthController(IConfiguration configuration, IJwtService jwtService, StoreDbContext storeDbContext)
        {
            _configuration = configuration;
            _jwtService = jwtService;
            _context = storeDbContext;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthLoginDTO authLoginDTO)
        {
            var user = _context.User.FirstOrDefault(user => user.Email.Equals(authLoginDTO.Email));
            var userIsValid = user != null;
            var passwordIsValid = userIsValid ? BC.Verify(authLoginDTO.Password, user.PasswordHashed) : false;

            if (!userIsValid || !passwordIsValid)
                return NotFound("The email or password is incorrect.");

            var jwtToken = _jwtService.GetJwtToken(user);
            return Ok(jwtToken);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                user.PasswordHashed = BC.HashPassword(user.Password);

                _context.Add(user);
                await _context.SaveChangesAsync();

                return Ok(user.ToDTO());
            }
            catch (DbUpdateException dbUpdateException)
            {
                return BadRequest(dbUpdateException.InnerException.Message);
            }

        }
    }
}