using VaggouAPI.DTOs;
using VaggouAPI.Models;

namespace VaggouAPI.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(UserDto dto);
    }
}
