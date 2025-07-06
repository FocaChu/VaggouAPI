using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(IImageService imageService, ILogger<ImagesController> logger)
        {
            _imageService = imageService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var imageData = await _imageService.GetImageAsync(id);
            if (imageData == null)
            {
                _logger.LogWarning("Image with ID: {ImageId} not found.", id);
                return NotFound();
            }
            return File(imageData.Value.content, imageData.Value.contentType);
        }

        [HttpPost("parking-lot/{parkingLotId}")]
        [Authorize(Roles = "ParkingLotOwner")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> UploadParkingLotImage(Guid parkingLotId, [FromForm] IFormFile file, [FromQuery] ImageType type)
        {
            if (type != ImageType.ParkingLotIcon && type != ImageType.ParkingLotBanner && type != ImageType.ParkingLotGallery)
                return BadRequest("Tipo de imagem inválido para estacionamento.");

            var userId = GetCurrentUserId();
            var image = await _imageService.UploadParkingLotImageAsync(parkingLotId, file, type, userId);

            _logger.LogInformation("User {UserId} uploaded image {ImageId} for parking lot {ParkingLotId}.", userId, image.Id, parkingLotId);
            return CreatedAtAction(nameof(GetImage), new { id = image.Id }, new { imageId = image.Id });
        }

        [HttpPost("client/profile")]
        [Authorize]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> UploadClientProfileImage([FromForm] IFormFile file)
        {
            var userId = GetCurrentUserId();
            var image = await _imageService.UploadClientProfileImageAsync(file, userId);

            _logger.LogInformation("User {UserId} uploaded new profile image {ImageId}.", userId, image.Id);
            return CreatedAtAction(nameof(GetImage), new { id = image.Id }, new { imageId = image.Id });
        }

        [HttpPost("vehicle/{vehicleId}")]
        [Authorize]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> UploadVehicleImage(Guid vehicleId, [FromForm] IFormFile file)
        {
            var userId = GetCurrentUserId();
            var image = await _imageService.UploadVehicleImageAsync(vehicleId, file, userId);

            _logger.LogInformation("User {UserId} uploaded image {ImageId} for vehicle {VehicleId}.", userId, image.Id, vehicleId);
            return CreatedAtAction(nameof(GetImage), new { id = image.Id }, new { imageId = image.Id });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            var userId = GetCurrentUserId();
            await _imageService.DeleteImageAsync(id, userId);

            _logger.LogInformation("User {UserId} deleted image {ImageId}.", userId, id);
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
