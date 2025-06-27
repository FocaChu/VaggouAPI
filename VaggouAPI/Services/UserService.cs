using Microsoft.EntityFrameworkCore;
using VaggouAPI.DTOs;
using VaggouAPI.Models;

namespace VaggouAPI.Services
{
    public class UserService
    {
        private readonly Db _context;

        public UserService(Db _context)
        {
            this._context = _context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new Exception("User Not Found");

            return user;
        }

        public async Task<User> CreateAsync(UserDto dto)
        {
            User user = new User()
            { 
                Id= Guid.NewGuid(),
                Email= dto.Email,
                Password= dto.Password,
                Role = dto.Role,
                IsActive= dto.IsActive,
            };

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(Guid id, UserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new Exception("User Not Found");

            user.Email = dto.Email;
            user.Password = dto.Password;
            user.Role = dto.Role;
            user.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new Exception("User Not Found");

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
