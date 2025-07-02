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
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService _service)
        {
            this._service = _service;
        }

        [HttpGet]
        public async Task<List<User>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _service.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<User> CreateAsync([FromBody] UserDto dto)
        {
            return await _service.CreateAsync(dto);
        }

        [HttpPut("{id}")]
        public async Task<User?> UpdateAsync(Guid id, [FromBody] UserDto dto)
        {
            return await _service.UpdateAsync(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _service.DeleteAsync(id);
        }
    }
}
