using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _service;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(IReservationService service, ILogger<ReservationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationRequestDto dto, Guid loggedInUserId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when creating reservation.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new reservation.");
            var created = await _service.CreateAsync(dto, loggedInUserId);
            _logger.LogInformation("Reservation created. ID: {Id}", created.Id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, Guid loggedInUserId)
        {
            _logger.LogInformation("Deleting reservation ID: {Id}", id);
            await _service.CancelAsync(id, loggedInUserId);
            _logger.LogInformation("Reservation deleted. ID: {Id}", id);
            return NoContent();
        }
    }
}