using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    public interface IAdressService
    {
        Task<IEnumerable<Address>> GetAllAsync();

        Task<Address?> GetByIdAsync(Guid id);

        Task<Address> CreateAsync(AddressDto dto);

        Task<Address?> UpdateAsync(AddressDto dto, Guid id);

        Task DeleteAsync(Guid id);
    }
}
