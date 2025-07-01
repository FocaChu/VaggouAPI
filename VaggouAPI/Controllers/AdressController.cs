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
            _logger.LogInformation("Listando todos os endereços.");
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Buscando endereço por ID: {Id}", id);
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdressDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao criar endereço.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Criando novo endereço.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Endereço criado. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] AdressDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao atualizar endereço ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Atualizando endereço ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Endereço atualizado. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deletando endereço ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Endereço deletado. ID: {Id}", id);
            return NoContent();
        }
    }
}
