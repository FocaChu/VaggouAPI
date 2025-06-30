using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    public interface IAdressService
    {
        Task<IEnumerable<Adress>> GetAllAsync();

        Task<Adress?> GetByIdAsync(Guid id);

        Task<Adress> CreateAsync(AdressDto dto);

        Task<Adress?> UpdateAsync(AdressDto dto, Guid id);

        Task<bool> DeleteAsync(Guid id);
    }
}
