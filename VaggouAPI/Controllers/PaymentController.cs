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
            return Ok(_service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Buscando pagamento com ID: {Id}", id);
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("paymentMethod/{paymentMethodId}")]
        public async Task<IActionResult> GetByZipCode(Guid paymentMethodId)
        {
            _logger.LogInformation("Buscando pagamentos pelo metodo de pagamento: {PaymentMethod}", paymentMethodId);
            return Ok(await _service.GetByPaymentMethodAsync(paymentMethodId));
        }

        [HttpGet("month")]
        public async Task<IActionResult> GetPaymentsByMonth([FromQuery] int year, [FromQuery] int month)
        {
            _logger.LogInformation("Buscando pagamento por mês: {Mounth}", month);
            return Ok(await _service.GetByMonthAsync(year, month));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao criar pagamento.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Criando novo pagamento.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Pagamento criado. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] PaymentDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao atualizar pagamento ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Atualizando pagamento ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Pagamento atualizado. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deletando pagamento ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Pagamento deletado. ID: {Id}", id);
            return NoContent();
        }
    }
}
