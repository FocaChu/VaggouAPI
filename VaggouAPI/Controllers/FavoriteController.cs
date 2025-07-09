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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Listing all favorites.");
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching favorite by ID: {Id}", id);

            var result = await _service.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetByClientId(Guid clientId)
        {
            _logger.LogInformation("Fetching favorite by Client ID: {ClientId}", clientId);

            var result = await _service.GetByClientIdAsync(clientId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FavoriteDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when creating favorite parking lot.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new favorite parking lot.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Favorite parking lot created. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] FavoriteDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao atualizar estacionamento favorito ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating favorite parking lot ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Favorite parking lot updated. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting favorite parking lot ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Address favorite parking lot. ID: {Id}", id);
            return NoContent();
        }
    }
}
