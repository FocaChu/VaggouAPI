using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _service;
        private readonly ILogger<FavoriteController> _logger;

        public FavoriteController(IFavoriteService service, ILogger<FavoriteController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetByClientId(Guid clientId)
        {
            _logger.LogInformation("Fetching favorite by Client ID: {ClientId}", clientId);

            var result = await _service.GetMyFavoritesAsync(clientId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFavoriteRequestDto dto, Guid clientId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when creating favorite parking lot.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new favorite parking lot.");
            var created = await _service.CreateAsync(clientId, dto);
            _logger.LogInformation("Favorite parking lot created. ID: {Id}", created.Id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, Guid clientId)
        {
            _logger.LogInformation("Deleting favorite parking lot ID: {Id}", id);
            await _service.DeleteAsync(id, clientId);
            _logger.LogInformation("Address favorite parking lot. ID: {Id}", id);
            return NoContent();
        }
    }
}
