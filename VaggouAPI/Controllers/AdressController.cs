using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdressController : ControllerBase
    {
        private readonly IAdressService _service;
        private readonly ILogger<AdressController> _logger;

        public AdressController(IAdressService service, ILogger<AdressController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Solicitada listagem de todos os endereços.");
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Buscando endereço com ID: {Id}", id);
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                _logger.LogWarning("Endereço com ID {Id} não encontrado.", id);
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdressDto dto)
        {
            try
            {
                _logger.LogInformation("Tentando adicionar endereço.");
                var created = await _service.CreateAsync(dto);
                _logger.LogInformation("Endereço criado com sucesso. ID: {Id}", created.Id);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("Erro ao criar endereço: {Mensagem}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar endereço.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] AdressDto dto, Guid id)
        {
            try
            {
                _logger.LogInformation("Tentando atualizar endereço ID: {Id}", id);
                var updated = await _service.UpdateAsync(dto, id);
                if (updated == null)
                {
                    _logger.LogWarning("Endereço com ID {Id} não encontrado para atualização.", id);
                    return NotFound();
                }

                _logger.LogInformation("Endereço atualizado com sucesso. ID: {Id}", id);
                return Ok(updated);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("Erro ao atualizar endereço: {Mensagem}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar endereço.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Tentando deletar endereço com ID: {Id}", id);
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning("Endereço ID {Id} não encontrado para deleção.", id);
                return NotFound();
            }

            _logger.LogInformation("Endereço ID {Id} deletado com sucesso.", id);
            return NoContent();
        }
    }
}
