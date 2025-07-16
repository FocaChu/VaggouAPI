namespace VaggouAPI
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAllAsync();

        Task<Address?> GetByIdAsync(Guid id);

        Task<Address> CreateAsync(CreateAddressRequestDto dto);

        Task<Address> UpdateAsync(Guid id, UpdateAddressRequestDto dto);

        Task DeleteAsync(Guid id);
    }
}
