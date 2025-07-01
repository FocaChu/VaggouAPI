using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _service;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(IReservationService service, ILogger<ReservationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Listando todos as reservas.");
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Buscando reserva por ID: {Id}", id);
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("month")]
        public async Task<IActionResult> GetPaymentsByMonth([FromQuery] int year, [FromQuery] int month)
        {
            _logger.LogInformation("Buscando reserva por mês: {Mounth}", month);
            return Ok(await _service.GetByMonthAsync(year, month));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReservationDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao criar reserva.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Criando nova reserva.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Reserva criado. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ReservationDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao atualizar reserva ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Atualizando reserva ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Reserva atualizada. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deletando reserva ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Reserva deletada. ID: {Id}", id);
            return NoContent();
        }
    }
}
