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
        [ProducesResponseType(typeof(ClientProfileDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("User {UserId} is fetching their profile.", userId);

            var profile = await _clientService.GetMyProfileAsync(userId);
            return Ok(profile);
        }

        [HttpPut("profile")]
        [ProducesResponseType(typeof(ClientProfileDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateClientProfileDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            _logger.LogInformation("User {UserId} is updating their profile.", userId);

            var updatedProfile = await _clientService.UpdateMyProfileAsync(userId, dto);
            return Ok(updatedProfile);
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
