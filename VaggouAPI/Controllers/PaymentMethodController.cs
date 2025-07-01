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
            _logger.LogInformation("Listando todos os metodos de pagamento.");
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Buscando metodo de pagamento por ID: {Id}", id);
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentMethodDto dto)
        {
            _logger.LogInformation("Criando novo metodo de pagamento.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Metodo de pagamento criado. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] PaymentMethodDto dto, Guid id)
        {
            _logger.LogInformation("Atualizando metodo de pagamento ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Metodo de pagamento atualizado. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deletando metodo de pagamento ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Metodo de pagamento deletado. ID: {Id}", id);
            return NoContent();
        }
    }
}
