using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService service, ILogger<PaymentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Solicitada listagem de todos os pagamentos.");
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Buscando pagamento com ID: {Id}", id);
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                _logger.LogWarning("Pagamento com ID {Id} não encontrado.", id);
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("paymentMethod/{paymentMethodId}")]
        public async Task<IActionResult> GetByZipCode(Guid paymentMethodId)
        {
            _logger.LogInformation("Buscando pagamentos pelo metodo de pagamento: {PaymentMethod}", paymentMethodId);
            var result = await _service.GetByPaymentMethodAsync(paymentMethodId);
            return Ok(result);
        }

        [HttpGet("month")]
        public async Task<IActionResult> GetPaymentsByMonth([FromQuery] int year, [FromQuery] int month)
        {
            var payments = await _service.GetByMonthAsync(year, month);
            return Ok(payments);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentDto dto)
        {
            try
            {
                _logger.LogInformation("Tentando criar um novo pagamento.");
                var created = await _service.CreateAsync(dto);
                _logger.LogInformation("Pagamento criado com sucesso. ID: {Id}", created.Id);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("Erro de negócio ao criar pagamento: {Mensagem}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar pagamento.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] PaymentDto dto, Guid id)
        {
            try
            {
                _logger.LogInformation("Tentando atualizar pagamento com ID: {Id}", id);
                var updated = await _service.UpdateAsync(dto, id);
                if (updated == null)
                {
                    _logger.LogWarning("Pagamento com ID {Id} não encontrado para atualização.", id);
                    return NotFound();
                }

                _logger.LogInformation("Pagamento atualizado com sucesso. ID: {Id}", id);
                return Ok(updated);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("Erro de negócio ao atualizar pagamento: {Mensagem}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar pagamento.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Tentando deletar pagamento com ID: {Id}", id);
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning("Pagamento com ID {Id} não encontrado para deleção.", id);
                return NotFound();
            }

            _logger.LogInformation("Pagamento com ID {Id} deletado com sucesso.", id);
            return NoContent();
        }
    }
}
