using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingLotController : ControllerBase
    {
        private readonly IParkingLotService _service;

        public ParkingLotController(IParkingLotService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("zip/{zipCode}")]
        public async Task<IActionResult> GetByZipCode(string zipCode)
        {
            var result = await _service.GetByAdressZipCodeAsync(zipCode);
            return Ok(result);
        }

        [HttpGet("proximity")]
        public async Task<IActionResult> GetByProximity(int latitude, int longitude, int raio)
        {
            var result = await _service.GetByProximityAsync(latitude, longitude, raio);
            return Ok(result);
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetByOwner(Guid ownerId)
        {
            var result = await _service.GetByOwnerIdAsync(ownerId);
            return Ok(result);
        }

        [HttpGet("with-cover")]
        public async Task<IActionResult> GetWithCover()
        {
            var result = await _service.GetWithCoverAsync();
            return Ok(result);
        }

        [HttpGet("with-pcd")]
        public async Task<IActionResult> GetWithPCDSpace()
        {
            var result = await _service.GetWithPCDSpaceAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParkingLotDto dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ParkingLotDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(dto, id);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
