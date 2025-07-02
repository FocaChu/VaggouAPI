using VaggouAPI.DTOs;
using VaggouAPI.Models;

namespace VaggouAPI.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
        Task<User> CreateAsync(UserDto dto);
        Task<User> UpdateAsync(Guid id, UserDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
