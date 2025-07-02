using VaggouAPI.DTOs;
using VaggouAPI.Models;

namespace VaggouAPI.Interfaces
{
    public interface IAuthService
    {
        Task<User> Register(User user);
        Task<User> Login(UserDto dto);
    }
}
