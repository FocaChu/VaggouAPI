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
        public async Task<IActionResult> Create([FromBody] CreateReviewRequestDto dto, Guid loggedInUserId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when creating review.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new review.");
            try
            {
                var created = await _service.CreateAsync(dto, loggedInUserId);
                _logger.LogInformation("Review created. ID: {Id}", created.Id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Failed to create review: {Message}", ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, Guid loggedInUserId)
        {
            _logger.LogInformation("Deleting review ID: {Id}", id);
            try
            {
                await _service.DeleteAsync(id, loggedInUserId);
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