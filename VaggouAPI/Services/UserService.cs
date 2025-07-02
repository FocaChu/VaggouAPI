using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using VaggouAPI.DTOs;
using VaggouAPI.Interfaces;
using VaggouAPI.Models;

namespace VaggouAPI.Services
{
    public class UserService:IUserService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public UserService(Db _context, IMapper _mapper, IAuthService _authService)
        {
            this._context = _context;
            this._mapper = _mapper;
            this._authService = _authService;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new BusinessException("User Not Found");

            return user;
        }

        public async Task<User> CreateAsync(UserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            //User user = new User()
            //{ 
            //    Id= Guid.NewGuid(),
            //    Email= dto.Email,
            //    Password= dto.Password,
            //    Role = dto.Role,
            //    IsActive= dto.IsActive,
            //};

            await _authService.Register(user);
            await _context.Users.AddAsync(user);
            
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(Guid id, UserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new BusinessException("User Not Found");

            user = _mapper.Map<User>(dto);
            //user.Email = dto.Email;
            //user.Password = dto.Password;
            //user.Role = dto.Role;
            //user.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new BusinessException("User Not Found");

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
