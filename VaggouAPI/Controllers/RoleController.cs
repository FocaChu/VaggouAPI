using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _service;
        private readonly ILogger<RoleController> _logger;

        public RoleController(RoleService service, ILogger<RoleController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Admin user is fetching all roles.");

            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"Admin user is fetching role with Id: {id}.");

            var result = await _service.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleDto dto)
        {
            _logger.LogInformation("Admin user is creating a new role.");

            var created = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] RoleDto dto, Guid id)
        {
            _logger.LogInformation($"Admin user is updating role with Id: {id}.");

            var updated = await _service.UpdateAsync(dto, id);

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Admin user is deleting role with Id: {id}.");
            await _service.DeleteAsync(id);
            _logger.LogInformation($"Deleted role with Id: {id}.");
            return NoContent();
        }
    }
}
