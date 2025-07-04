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
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching vehicle by ID: {Id}", id);
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetByOwner(Guid ownerId)
        {
            _logger.LogInformation("Fetching vehicles by owner ID: {OwnerId}", ownerId);
            return Ok(await _service.GetByOwnerIdAsync(ownerId));
        }

        [HttpGet("model/{modelId}")]
        public async Task<IActionResult> GetByModel(Guid modelId)
        {
            _logger.LogInformation("Fetching vehicles by model ID: {ModelId}", modelId);
            return Ok(await _service.GetByModelIdAsync(modelId));
        }

        [HttpGet("pre-registered")]
        public async Task<IActionResult> GetPreRegistered()
        {
            _logger.LogInformation("Fetching pre-registered vehicles.");
            return Ok(await _service.GetPreRegisteredAsync());
        }

        [HttpGet("plate/{plate}")]
        public async Task<IActionResult> GetByLicensePlate(string plate)
        {
            _logger.LogInformation("Fetching vehicle by license plate: {Plate}", plate);
            return Ok(await _service.GetByLicensePlateAsync(plate));
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