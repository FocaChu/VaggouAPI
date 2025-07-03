using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaggouAPI.DTOs;
using VaggouAPI.Interfaces;
using VaggouAPI.Models;
using VaggouAPI.Services;

namespace VaggouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _service;

        public ClientController(IClientService _service)
        {
            this._service = _service;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<Client>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<Client> GetByIdAsync(Guid id)
        {
            return await _service.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<Client>> CreateAsync(ClientDto dto)
        {
            try
            {
                return Ok(await _service.CreateAsync(dto));
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<Client> UpdateAsync(Guid id, ClientDto dto)
        {
            return await _service.UpdateAsync(id, dto);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _service.DeleteAsync(id);
        }
    }
}
