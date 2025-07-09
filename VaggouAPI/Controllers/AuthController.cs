using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService service, ILogger<AuthController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("register")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("Registering user.");
            return Ok(await _service.RegisterAsync(registerDto));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("Login user.");
            return Ok(await _service.Login(loginDto));
        }
    }
}
