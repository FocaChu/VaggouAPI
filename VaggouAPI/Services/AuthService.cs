using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VaggouAPI.DTOs;
using VaggouAPI.Models;

namespace VaggouAPI.Services
{
    public class AuthService
    {
        private readonly Db _context;
        private IConfiguration _configuration;

        public AuthService(IConfiguration _configuration, Db _context)
        {
            this._configuration = _configuration;
            this._context = _context;
        }

        //post por padrao o get deixa no cache a senha dai todos poem ver por isso post
        public async Task<string> Login(UserDto dto)
        {
            //crio atributo chave de acesso e taco la
            var user = await _context.Users.FindAsync(dto.Email);

            string email = user.Email;
            if (email == null) throw new BusinessException("Invalid Email");

            string password = user.Password;
            if (password == null) throw new BusinessException("Invalid Password");

            var token = GenerateToken(user);

            return "";

        }


        private async Task<string> GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity
                (
                    new[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),

                Expires = DateTime.UtcNow.AddDays(14),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],

                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //converte o objeto JwtSecurityToken em uma string JWT compactada
            var JwtToken = tokenHandler.WriteToken(token);
            return JwtToken;
        }
    }
}
