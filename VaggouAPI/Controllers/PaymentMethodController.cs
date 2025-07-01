using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _service;
        private readonly ILogger<PaymentMethodController> _logger;

        public PaymentMethodController(IPaymentMethodService service, ILogger<PaymentMethodController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Solicitada listagem de todos os metodos de pagamento.");
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Buscando metodo de pagamento com ID: {Id}", id);
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                _logger.LogWarning("Metodo de pagamento com ID {Id} não encontrado.", id);
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentMethodDto dto)
        {
            try
            {
                _logger.LogInformation("Tentando adicionar metodo de pagamento.");
                var created = await _service.CreateAsync(dto);
                _logger.LogInformation("Metodo de pagamento criado com sucesso. ID: {Id}", created.Id);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("Erro ao criar metodo de pagamento: {Mensagem}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar metodo de pagamento.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] PaymentMethodDto dto, Guid id)
        {
            try
            {
                _logger.LogInformation("Tentando atualizar metodo de pagamento com ID: {Id}", id);
                var updated = await _service.UpdateAsync(dto, id);
                if (updated == null)
                {
                    _logger.LogWarning("Metodo de pagamento com ID {Id} não encontrado para atualização.", id);
                    return NotFound();
                }

                _logger.LogInformation("Metodo de pagamento atualizado com sucesso. ID: {Id}", id);
                return Ok(updated);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("Erro ao atualizar metodo de pagamento: {Mensagem}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar metodo de pagamento.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Tentando deletar metodo de pagamento com ID: {Id}", id);
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning("Metodo de pagamento com ID {Id} não encontrado para deleção.", id);
                return NotFound();
            }

            _logger.LogInformation("Metodo de pagamento com ID {Id} deletado com sucesso.", id);
            return NoContent();
        }
    }
}
