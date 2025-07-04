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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Listing all reservations.");
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching reservation by ID: {Id}", id);
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("month")]
        public async Task<IActionResult> GetPaymentsByMonth([FromQuery] int year, [FromQuery] int month)
        {
            _logger.LogInformation("Fetching reservation by month: {Month}", month);
            return Ok(await _service.GetByMonthAsync(year, month));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReservationDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when creating reservation.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new reservation.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Reservation created. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ReservationDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when updating reservation ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating reservation ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Reservation updated. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting reservation ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Reservation deleted. ID: {Id}", id);
            return NoContent();
        }
    }
}