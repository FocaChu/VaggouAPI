namespace VaggouAPI
{
    public interface IVehicleModelService
    {
        Task<IEnumerable<VehicleModel>> GetAllAsync();

        Task<VehicleModel> GetByIdAsync(Guid id);

        Task<IEnumerable<VehicleModel>> GetByFuelTypeAsync(FuelType fuelType);

        Task<VehicleModel> CreateAsync(VehicleModelDto dto);

        Task<VehicleModel> UpdateAsync(VehicleModelDto dto, Guid id);

        Task DeleteAsync(Guid id);
    }
}
