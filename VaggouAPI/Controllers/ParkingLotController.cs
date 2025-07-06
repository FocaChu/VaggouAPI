using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VaggouAPI.DTOs.ParkingLot;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingLotsController : ControllerBase
    {
        private readonly IParkingLotService _service;
        private readonly ITokenService _tokenService; 
        private readonly ILogger<ParkingLotsController> _logger;

        public ParkingLotsController(IParkingLotService service, ITokenService tokenService, ILogger<ParkingLotsController> logger)
        {
            _service = service;
            _tokenService = tokenService; 
            _logger = logger;
        }

        [HttpGet("score")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllByScore()
        {
            _logger.LogInformation("Listing all parking lots.");
            return Ok(await _service.GetAllByScoreAsync());
        }

        [HttpGet("proximity/{latitude}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllByProximity(double latitude, double longitude)
        {
            _logger.LogInformation("Listing all parking lots.");
            return Ok(await _service.GetAllByProximityAsync(latitude, longitude));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching parking lot by ID: {Id}", id);
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("zip/{zipCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByZipCode(string zipCode)
        {
            _logger.LogInformation("Fetching parking lots by Zip Code: {ZipCode}", zipCode);
            return Ok(await _service.GetByAdressZipCodeAsync(zipCode));
        }

        [HttpGet("proximity")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByProximity([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double raio)
        {
            _logger.LogInformation("Fetching by proximity (Lat: {Latitude}, Long: {Longitude}, Radius: {Radius})", latitude, longitude, raio);
            return Ok(await _service.GetByProximityAsync(latitude, longitude, raio));
        }

        [HttpGet("owner/{ownerId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByOwner(Guid ownerId)
        {
            _logger.LogInformation("Fetching parking lots by owner ID: {OwnerId}", ownerId);
            return Ok(await _service.GetByOwnerIdAsync(ownerId));
        }

        [HttpGet("with-cover")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWithCover()
        {
            _logger.LogInformation("Fetching parking lots with cover.");
            return Ok(await _service.GetWithCoverAsync());
        }

        [HttpGet("with-pcd")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWithPCDSpace()
        {
            _logger.LogInformation("Fetching parking lots with accessible parking space.");
            return Ok(await _service.GetWithPCDSpaceAsync());
        }

        [HttpGet("my-lots")]
        [Authorize(Roles = "ParkingLotOwner")] 
        public async Task<IActionResult> GetMyParkingLots()
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("Fetching parking lots for owner ID: {OwnerId}", userId);
            var parkingLots = await _service.GetMyParkingLotsAsync(userId);
            return Ok(parkingLots);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] ParkingLotDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            _logger.LogInformation("User {UserId} is creating a new parking lot.", userId);

            var (createdParkingLot, updatedUser, roleWasAdded) = await _service.CreateAsync(dto, userId);

            string? newToken = null;
            if (roleWasAdded)
            {
                _logger.LogInformation("User {UserId} was granted 'ParkingLotOwner' role. Generating new token.", userId);
                newToken = _tokenService.GenerateToken(updatedUser);
            }

            var response = new CreateParkingLotResponseDto
            {
                CreatedParkingLot = createdParkingLot,
                NewToken = newToken
            };

            return CreatedAtAction(nameof(GetById), new { id = createdParkingLot.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ParkingLotOwner")]
        public async Task<IActionResult> Update([FromBody] ParkingLotDto dto, Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            _logger.LogInformation("User {UserId} is updating parking lot ID: {ParkingLotId}", userId, id);

            var updated = await _service.UpdateAsync(dto, id, userId);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ParkingLotOwner")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetCurrentUserId();
            _logger.LogInformation("User {UserId} is deleting parking lot ID: {ParkingLotId}", userId, id);

            await _service.DeleteAsync(id, userId);
            return NoContent();
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("ID de usuário inválido no token.");
            }
            return userId;
        }
    }
}