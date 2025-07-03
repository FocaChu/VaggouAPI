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
        

        [HttpPost("login")]
        public async Task<string> Login(UserDto dto)
        {
            var token = await _service.Login(dto);
            return token;
        }
    }
}
