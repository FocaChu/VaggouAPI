using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(IReviewService service, ILogger<ReviewController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByScore()
        {
            _logger.LogInformation("Listing all reviews.");
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching review by ID: {Id}", id);
            try
            {
                var review = await _service.GetByIdAsync(id);
                return Ok(review);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Review with ID {Id} not found: {Message}", id, ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetByClient(Guid clientId)
        {
            _logger.LogInformation("Fetching reviews by client ID: {ClientId}", clientId);
            var review = await _service.GetByClientAsync(clientId);
            if (review == null)
            {
                _logger.LogInformation("No review found for client ID: {ClientId}", clientId);
                return NotFound($"No review found for client ID: {clientId}");
            }
            return Ok(review);
        }

        [HttpGet("parkingLot/{parkingLotId}")]
        public async Task<IActionResult> GetByParkingLot(Guid parkingLotId)
        {
            _logger.LogInformation("Fetching reviews by parking lot ID: {ParkingLotId}", parkingLotId);
            var review = await _service.GetByParkingLotAsync(parkingLotId);
            if (review == null)
            {
                _logger.LogInformation("No review found for parking lot ID: {ParkingLotId}", parkingLotId);
                return NotFound($"No review found for parking lot ID: {parkingLotId}");
            }
            return Ok(review);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReviewDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when creating review.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new review.");
            try
            {
                var created = await _service.CreateAsync(dto);
                _logger.LogInformation("Review created. ID: {Id}", created.Id);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Failed to create review: {Message}", ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ReviewDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when updating review ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating review ID: {Id}", id);
            try
            {
                var updated = await _service.UpdateAsync(dto, id);
                _logger.LogInformation("Review updated. ID: {Id}", updated.Id);
                return Ok(updated);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Failed to update review ID {Id}: {Message}", id, ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting review ID: {Id}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Review deleted. ID: {Id}", id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Failed to delete review ID {Id}: {Message}", id, ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}