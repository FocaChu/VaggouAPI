using VaggouAPI.DTOs;
using VaggouAPI.Models;

namespace VaggouAPI.Interfaces
{
    public interface IClientService
    {
         Task<List<Client>> GetAllAsync();
         Task<Client> GetByIdAsync(Guid id);
         Task<Client> CreateAsync(ClientDto dto);
         Task<Client> UpdateAsync(Guid id, ClientDto dto);
         Task<bool> DeleteAsync(Guid id);

    }
}
