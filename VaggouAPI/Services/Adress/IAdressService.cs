using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    public interface IAdressService
    {
        Task<IEnumerable<Adress>> GetAllAsync();

        Task<Adress?> GetByIdAsync(Guid id);

        Task<Adress> CreateAsync([FromBody] AdressDto dto);

        Task<Adress?> UpdateAsync([FromBody] AdressDto dto, Guid id);

        Task<bool> DeleteAsync(Guid id);
    }
}
