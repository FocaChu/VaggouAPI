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
            _logger.LogInformation("Listando todos os modelos de veículo.");
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Buscando modelo de veículo por ID: {Id}", id);
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("fuel-type/{fuelType}")]
        public async Task<IActionResult> GetByFuelType(FuelType fuelType)
        {
            _logger.LogInformation("Buscando modelos de veículo por tipo de combustível: {FuelType}", fuelType);
            return Ok(await _service.GetByFuelTypeAsync(fuelType));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleModelDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao criar modelo de veículo.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Criando novo modelo de veículo.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Modelo criado. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] VehicleModelDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao atualizar modelo ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Atualizando modelo ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Modelo atualizado. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deletando modelo ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Modelo deletado. ID: {Id}", id);
            return NoContent();
        }
    }
}
