using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingSpotController : ControllerBase
    {
        private readonly IParkingSpotService _service;
        private readonly ILogger<ParkingSpotController> _logger;

        public ParkingSpotController(IParkingSpotService service, ILogger<ParkingSpotController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Listing all parking spots.");

            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching parking spot by ID: {Id}", id);

            var result = await _service.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("parkingLot/{id}")]
        public async Task<IActionResult> GetByParkingLotId(Guid id)
        {
            _logger.LogInformation("Fetching parking spots in parking lot with ID: {Id}", id);

            var result = await _service.GetByParkingLotIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParkingSpotDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when creating parking spot.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new parking spot.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Parking spot created. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ParkingSpotDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when updating parking spot ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating parking spot ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Parking spot updated. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting parking spot ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Parking spot deleted. ID: {Id}", id);
            return NoContent();
        }
    }
}