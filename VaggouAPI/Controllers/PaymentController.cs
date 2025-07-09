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
            _logger.LogInformation("Requested listing of all payments.");

            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching payment with ID: {Id}", id);

            var result = await _service.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("paymentMethod/{paymentMethodId}")]
        public async Task<IActionResult> GetByZipCode(Guid paymentMethodId)
        {
            _logger.LogInformation("Fetching payments by payment method: {PaymentMethod}", paymentMethodId);

            var result = await _service.GetByPaymentMethodAsync(paymentMethodId);

            return Ok(result);
        }

        [HttpGet("month")]
        public async Task<IActionResult> GetPaymentsByMonth([FromQuery] int year, [FromQuery] int month)
        {
            _logger.LogInformation("Fetching payments by month: {Month}", month);

            var result = await _service.GetByMonthAsync(year, month);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when creating payment.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new payment.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Payment created. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] PaymentDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when updating payment ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating payment ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Payment updated. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting payment ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Payment deleted. ID: {Id}", id);
            return NoContent();
        }
    }
}