using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI.Controllers
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
            _logger.LogInformation("Solicitada listagem de todos os estacionamentos.");
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Buscando estacionamento com ID: {Id}", id);
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                _logger.LogWarning("Estacionamento com ID {Id} não encontrado.", id);
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("zip/{zipCode}")]
        public async Task<IActionResult> GetByZipCode(string zipCode)
        {
            _logger.LogInformation("Buscando estacionamentos pelo CEP: {ZipCode}", zipCode);
            var result = await _service.GetByAdressZipCodeAsync(zipCode);
            return Ok(result);
        }

        [HttpGet("proximity")]
        public async Task<IActionResult> GetByProximity(int latitude, int longitude, int raio)
        {
            _logger.LogInformation("Buscando por proximidade: lat {Latitude}, long {Longitude}, raio {Raio}m", latitude, longitude, raio);
            var result = await _service.GetByProximityAsync(latitude, longitude, raio);
            return Ok(result);
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetByOwner(Guid ownerId)
        {
            _logger.LogInformation("Buscando estacionamentos do dono ID: {OwnerId}", ownerId);
            var result = await _service.GetByOwnerIdAsync(ownerId);
            return Ok(result);
        }

        [HttpGet("with-cover")]
        public async Task<IActionResult> GetWithCover()
        {
            _logger.LogInformation("Buscando estacionamentos com cobertura.");
            var result = await _service.GetWithCoverAsync();
            return Ok(result);
        }

        [HttpGet("with-pcd")]
        public async Task<IActionResult> GetWithPCDSpace()
        {
            _logger.LogInformation("Buscando estacionamentos com vaga PCD.");
            var result = await _service.GetWithPCDSpaceAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParkingLotDto dto)
        {
            try
            {
                _logger.LogInformation("Tentando criar um novo estacionamento.");
                var created = await _service.CreateAsync(dto);
                _logger.LogInformation("Estacionamento criado com sucesso. ID: {Id}", created.Id);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("Erro de negócio ao criar estacionamento: {Mensagem}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar estacionamento.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ParkingLotDto dto, Guid id)
        {
            try
            {
                _logger.LogInformation("Tentando atualizar estacionamento ID: {Id}", id);
                var updated = await _service.UpdateAsync(dto, id);
                if (updated == null)
                {
                    _logger.LogWarning("Estacionamento ID {Id} não encontrado para atualização.", id);
                    return NotFound();
                }

                _logger.LogInformation("Estacionamento atualizado com sucesso. ID: {Id}", id);
                return Ok(updated);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("Erro de negócio ao atualizar estacionamento: {Mensagem}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar estacionamento.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Tentando deletar estacionamento ID: {Id}", id);
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning("Estacionamento ID {Id} não encontrado para deleção.", id);
                return NotFound();
            }

            _logger.LogInformation("Estacionamento ID {Id} deletado com sucesso.", id);
            return NoContent();
        }
    }
}
