using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _service;

        public FavoriteController(IFavoriteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var favorites = await _service.GetAllAsync();
            return Ok(favorites);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var favorite = await _service.GetByIdAsync(id);
            return favorite == null ? NotFound() : Ok(favorite);
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetByClientId(Guid clientId)
        {
            var favorites = await _service.GetByClientIdAsync(clientId);
            return Ok(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FavoriteDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var favorite = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = favorite.Id }, favorite);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] FavoriteDto dto, Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var favorite = await _service.UpdateAsync(dto, id);
            return favorite == null ? NotFound() : Ok(favorite);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
