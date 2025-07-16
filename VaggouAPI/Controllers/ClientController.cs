using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IClientService clientService, ILogger<ClientController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        [HttpGet("profile")]
        [ProducesResponseType(typeof(ClientProfileResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("User {UserId} is fetching their profile.", userId);

            var result = await _clientService.GetMyProfileAsync(userId);
            return Ok(result);
        }

        [HttpPut("profile")]
        [ProducesResponseType(typeof(ClientProfileResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateClientProfileRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            _logger.LogInformation("User {UserId} is updating their profile.", userId);

            var result = await _clientService.UpdateMyProfileAsync(userId, dto);
            return Ok(result);
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid user ID in the token.");
            }
            return userId;
        }
    }
}
