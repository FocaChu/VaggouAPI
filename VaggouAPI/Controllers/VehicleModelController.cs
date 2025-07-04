using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleModelsController : ControllerBase
    {
        private readonly IVehicleModelService _service;
        private readonly ILogger<VehicleModelsController> _logger;

        public VehicleModelsController(IVehicleModelService service, ILogger<VehicleModelsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Listing all vehicle models.");
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching vehicle model by ID: {Id}", id);
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("fuel-type/{fuelType}")]
        public async Task<IActionResult> GetByFuelType(FuelType fuelType)
        {
            _logger.LogInformation("Fetching vehicle models by fuel type: {FuelType}", fuelType);
            return Ok(await _service.GetByFuelTypeAsync(fuelType));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleModelDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when creating vehicle model.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new vehicle model.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Model created. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] VehicleModelDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when updating model ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating model ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Model updated. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting model ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Model deleted. ID: {Id}", id);
            return NoContent();
        }
    }
}