using Microsoft.AspNetCore.Http;
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
            _logger.LogInformation("Solicitada listagem de todos as vagas.");
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Buscando vaga com ID: {Id}", id);
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                _logger.LogWarning("Vaga com ID {Id} não encontrado.", id);
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("parkingLot/{id}")]
        public async Task<IActionResult> GetByParkingLotId(Guid id)
        {
            _logger.LogInformation("Buscando vagas em estacionamento com ID: {Id}", id);
            var result = await _service.GetByParkingLotIdAsync(id);
            if (result == null)
            {
                _logger.LogWarning("Estacionamento com ID {Id} não encontrado.", id);
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParkingSpotDto dto)
        {
            try
            {
                _logger.LogInformation("Tentando adicionar vaga. ParkingLotId: {ParkingLotId}", dto.ParkingLotId);
                var created = await _service.CreateAsync(dto);
                _logger.LogInformation("Favorito criado com sucesso. ID: {Id}", created.Id);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("Erro ao criar vaga: {Mensagem}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar vaga.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ParkingSpotDto dto, Guid id)
        {
            try
            {
                _logger.LogInformation("Tentando atualizar vaga com ID: {Id}", id);
                var updated = await _service.UpdateAsync(dto, id);
                if (updated == null)
                {
                    _logger.LogWarning("Vaga com ID {Id} não encontrada para atualização.", id);
                    return NotFound();
                }

                _logger.LogInformation("Vaga atualizada com sucesso. ID: {Id}", id);
                return Ok(updated);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("Erro ao atualizar vaga: {Mensagem}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar vaga.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Tentando deletar vaga com ID: {Id}", id);
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning("Vaga com ID {Id} não encontrada para deleção.", id);
                return NotFound();
            }

            _logger.LogInformation("Vaga com ID {Id} deletada com sucesso.", id);
            return NoContent();
        }
    }
}
