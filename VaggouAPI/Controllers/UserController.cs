using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserSummaryDto>), 200)]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Admin user is fetching a list of all users.");

            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDetailDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            _logger.LogInformation("Admin user is fetching details for user ID: {UserId}", id);

            var user = await _userService.GetUserByIdAsync(id);

            return Ok(user);
        }

        [HttpPut("{id}/roles")]
        [ProducesResponseType(204)] // No Content
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(404)] // Not Found
        public async Task<IActionResult> UpdateUserRoles(Guid id, [FromBody] UpdateUserRolesDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Admin user is updating roles for user ID: {UserId}", id);
            await _userService.UpdateUserRolesAsync(id, dto);

            return NoContent();
        }
    }
}
