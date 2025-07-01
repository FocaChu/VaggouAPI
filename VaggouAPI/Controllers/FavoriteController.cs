using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _service;
        private readonly ILogger<FavoriteController> _logger;

        public FavoriteController(IFavoriteService service, ILogger<FavoriteController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Listando todos os estacionamentos favoritados.");
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Buscando estacionamento favorito por ID: {Id}", id);
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetByClientId(Guid clientId)
        {
            _logger.LogInformation("Buscando favoritos do cliente ID: {ClientId}", clientId);
            return Ok(await _service.GetByClientIdAsync(clientId));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FavoriteDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao criar estacionamento favorito.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Criando novo estacionamento favorito.");
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Estacionamento favorito criado. ID: {Id}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] FavoriteDto dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Erro de validação ao atualizar estacionamento favorito ID: {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Atualizando estacionamento favorito ID: {Id}", id);
            var updated = await _service.UpdateAsync(dto, id);
            _logger.LogInformation("Estacionamento atualizado favorito. ID: {Id}", updated.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deletando estacionamento favorito ID: {Id}", id);
            await _service.DeleteAsync(id);
            _logger.LogInformation("Estacionamento favorito deletado. ID: {Id}", id);
            return NoContent();
        }
    }
}
