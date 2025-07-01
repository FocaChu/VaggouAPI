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
            _logger.LogInformation("Listando todos as vagas.");
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Buscando vaga por ID: {Id}", id);
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("parkingLot/{id}")]
        public async Task<IActionResult> GetByParkingLotId(Guid id)
        {
            _logger.LogInformation("Buscando vagas em estacionamento com ID: {Id}", id);
            return Ok(await _service.GetByParkingLotIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParkingSpotDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao criar vaga.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Criando nova vaga.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Vaga criada. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ParkingSpotDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao atualizar vaga ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Atualizando vaga ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Vaga atualizada. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deletando vaga ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Vaga deletada. ID: {Id}", id);
            return NoContent();
        }
    }
}
