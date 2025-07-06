using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Db _context;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthController(Db context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return BadRequest("This email is already in use.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = registerDto.Email,
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

            var client = new Client
            {
                Id = user.Id,
                FullName = registerDto.FullName,
                Phone = registerDto.Phone,
                CPF = registerDto.CPF,
                User = user
            };

            var consumerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Consumer");
            if (consumerRole == null) { return StatusCode(500, "Role 'Consumer' not found."); }
            user.Roles.Add(consumerRole);

            // 6. Salvar no banco
            _context.Users.Add(user);
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email or password."); 
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _tokenService.GenerateToken(user);

            var clientProfile = await _context.Clients
                .Where(c => c.Id == user.Id)
                .Select(c => new ClientDto { FullName = c.FullName, Email = user.Email, Phone = c.Phone, CPF = c.CPF })
                .FirstOrDefaultAsync();

            // 5. Retornar o token e o perfil
            return Ok(new AuthResponseDto
            {
                Token = token,
                UserProfile = clientProfile
            });
        }
    }
}
