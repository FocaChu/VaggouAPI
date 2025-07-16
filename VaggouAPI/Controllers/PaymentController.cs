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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, Guid loggedInUserId)
        {
            _logger.LogInformation("Fetching payment with ID: {Id}", id);

            var result = await _service.GetByIdAsync(id, loggedInUserId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InitiatePaymentRequestDto dto, Guid loggedInUserId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when creating payment.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new payment.");
            var created = await _service.InitiatePaymentForReservationAsync(dto, loggedInUserId);
            _logger.LogInformation("Payment created. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Status status)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error when updating payment ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating payment ID: {Id}", id);
            var updated = await _service.UpdatePaymentStatusAsync(id, status);
            _logger.LogInformation("Payment updated. ID: {Id}", updated.Id);
            return Ok(updated);
        }
    }
}