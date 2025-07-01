namespace VaggouAPI
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();

        Task<Vehicle> GetByIdAsync(Guid id);

        Task<IEnumerable<Vehicle>> GetByOwnerIdAsync(Guid ownerId);

        Task<IEnumerable<Vehicle>> GetByModelIdAsync(Guid modelId);

        Task<IEnumerable<Vehicle>> GetPreRegisteredAsync();

        Task<Vehicle> GetByLicensePlateAsync(string plate);

        Task<Vehicle> CreateAsync(VehicleDto dto);

        Task<Vehicle> UpdateAsync(VehicleDto dto, Guid id);

        Task DeleteAsync(Guid id);
    }
}
