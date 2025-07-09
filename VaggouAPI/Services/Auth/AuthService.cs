using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;

namespace VaggouAPI
{
    public class AuthService : IAuthService
    {
        private readonly Db _context;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(Db context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                throw new BusinessException("This email is already in use.");
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

            var consumerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Consumer")
                ?? throw new BusinessException("Role 'Consumer' not found.");

            user.Roles.Add(consumerRole);

            _context.Users.Add(user);
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return "Registration successful"; 
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            var token = _tokenService.GenerateToken(user);

            var clientProfile = await _context.Clients
                .Where(c => c.Id == user.Id)
                .Select(c => new ClientDto { FullName = c.FullName, Email = user.Email, Phone = c.Phone, CPF = c.CPF })
                .FirstOrDefaultAsync();

            return new AuthResponseDto
            {
                Token = token,
                UserProfile = clientProfile
            };
        }
    }
}
