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
            _logger.LogInformation("Listando todos os veículos.");
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Buscando veículo por ID: {Id}", id);
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetByOwner(Guid ownerId)
        {
            _logger.LogInformation("Buscando veículos do proprietário ID: {OwnerId}", ownerId);
            return Ok(await _service.GetByOwnerIdAsync(ownerId));
        }

        [HttpGet("model/{modelId}")]
        public async Task<IActionResult> GetByModel(Guid modelId)
        {
            _logger.LogInformation("Buscando veículos por modelo ID: {ModelId}", modelId);
            return Ok(await _service.GetByModelIdAsync(modelId));
        }

        [HttpGet("pre-registered")]
        public async Task<IActionResult> GetPreRegistered()
        {
            _logger.LogInformation("Buscando veículos pré-cadastrados.");
            return Ok(await _service.GetPreRegisteredAsync());
        }

        [HttpGet("plate/{plate}")]
        public async Task<IActionResult> GetByLicensePlate(string plate)
        {
            _logger.LogInformation("Buscando veículo pela placa: {Plate}", plate);
            return Ok(await _service.GetByLicensePlateAsync(plate));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao criar veículo.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Criando novo veículo.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Veículo criado. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] VehicleDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao atualizar veículo ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Atualizando veículo ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Veículo atualizado. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deletando veículo ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Veículo deletado. ID: {Id}", id);
            return NoContent();
        }
    }
}
