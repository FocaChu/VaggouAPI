using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaggouAPI.DTOs;
using VaggouAPI.Interfaces;
using VaggouAPI.Models;

namespace VaggouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthService _service;

        public AuthController(IAuthService _service)
        {
            this._service = _service;
        }
        [HttpPost("cadastro")]
        public async Task<User> Register(User user)
        {
            User TokenAcess = await _service.Register(user);
            return TokenAcess;
        }

        [HttpPost("login")]
        [Authorize]
        public async Task<User> Login(UserDto dto)
        {
            User verifyAcess = await _service.Login(dto);
            return verifyAcess;
        }
    }
}
