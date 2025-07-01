using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingLotsController : ControllerBase
    {
        private readonly IParkingLotService _service;
        private readonly ILogger<ParkingLotsController> _logger;

        public ParkingLotsController(IParkingLotService service, ILogger<ParkingLotsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Listando todos os estacionamentos.");
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Buscando estacionamento por ID: {Id}", id);
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("zip/{zipCode}")]
        public async Task<IActionResult> GetByZipCode(string zipCode)
        {
            _logger.LogInformation("Buscando estacionamentos pelo CEP: {ZipCode}", zipCode);
            return Ok(await _service.GetByAdressZipCodeAsync(zipCode));
        }

        [HttpGet("proximity")]
        public async Task<IActionResult> GetByProximity([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double raio)

        {
            _logger.LogInformation("Buscando por proximidade (Lat: {Latitude}, Long: {Longitude}, Raio: {Raio})", latitude, longitude, raio);
            return Ok(await _service.GetByProximityAsync(latitude, longitude, raio));
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetByOwner(Guid ownerId)
        {
            _logger.LogInformation("Buscando estacionamentos do dono ID: {OwnerId}", ownerId);
            return Ok(await _service.GetByOwnerIdAsync(ownerId));
        }

        [HttpGet("with-cover")]
        public async Task<IActionResult> GetWithCover()
        {
            _logger.LogInformation("Buscando estacionamentos com cobertura.");
            return Ok(await _service.GetWithCoverAsync());
        }

        [HttpGet("with-pcd")]
        public async Task<IActionResult> GetWithPCDSpace()
        {
            _logger.LogInformation("Buscando estacionamentos com vaga PCD.");
            return Ok(await _service.GetWithPCDSpaceAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParkingLotDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao criar estacionamento.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Criando novo estacionamento.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Estacionamento criado. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ParkingLotDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao atualizar estacionamento ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Atualizando estacionamento ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Estacionamento atualizado. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deletando estacionamento ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Estacionamento deletado. ID: {Id}", id);
            return NoContent();
        }
    }
}
