using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    public interface IParkingSpotService
    {
        Task<IEnumerable<ParkingSpot>> GetAllAsync();

        Task<ParkingSpot?> GetByIdAsync(Guid id);

        Task<IEnumerable<ParkingSpot>> GetByParkingLotIdAsync(Guid parkingLotId);

        Task<ParkingSpot> CreateAsync(ParkingSpotDto dto);

        Task<ParkingSpot?> UpdateAsync(ParkingSpotDto dto, Guid Id);

        Task DeleteAsync(Guid id);
    }
}
