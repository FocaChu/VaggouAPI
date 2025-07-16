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
            _logger.LogInformation("Listing all payment methods.");

            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching payment method by ID: {Id}", id);

            var result = await _service.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentMethodRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when creating payment method.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new payment method.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Payment method created. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] CreatePaymentMethodRequestDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when updating payment method ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating payment method ID: {Id}", id);
            var updated = await _service.UpdateAsync(id, dto);
            _logger.LogInformation("Payment method updated. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting payment method ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Payment method deleted. ID: {Id}", id);
            return NoContent();
        }
    }
}