using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _service;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IVehicleService service, ILogger<VehiclesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Listing all vehicles.");

            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching vehicle by ID: {Id}", id);

            var result = await _service.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetByOwner(Guid ownerId)
        {
            _logger.LogInformation("Fetching vehicles by owner ID: {OwnerId}", ownerId);

            var result = await _service.GetByOwnerIdAsync(ownerId);

            return Ok(result);
        }

        [HttpGet("model/{modelId}")]
        public async Task<IActionResult> GetByModel(Guid modelId)
        {
            _logger.LogInformation("Fetching vehicles by model ID: {ModelId}", modelId);

            var result = await _service.GetByModelIdAsync(modelId);

            return Ok(result);
        }

        [HttpGet("pre-registered")]
        public async Task<IActionResult> GetPreRegistered()
        {
            _logger.LogInformation("Fetching pre-registered vehicles."); 
            
            var result = await _service.GetPreRegisteredAsync();

            return Ok(result);
        }

        [HttpGet("plate/{plate}")]
        public async Task<IActionResult> GetByLicensePlate(string plate)
        {
            _logger.LogInformation("Fetching vehicle by license plate: {Plate}", plate);

            var result = await _service.GetPreRegisteredAsync();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when creating vehicle.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new vehicle.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Vehicle created. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] VehicleDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when updating vehicle ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating vehicle ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Vehicle updated. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting vehicle ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Vehicle deleted. ID: {Id}", id);
            return NoContent();
        }
    }
}