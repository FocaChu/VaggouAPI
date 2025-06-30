using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    public interface IParkingLotService
    {
        Task<IEnumerable<ParkingLot>> GetAllAsync();

        Task<ParkingLot?> GetByIdAsync(Guid id);

        Task<IEnumerable<ParkingLot>> GetByAdressZipCodeAsync(string zipCode);

        Task<IEnumerable<ParkingLot>> GetByProximityAsync(int latitude, int longitude, int raio);

        Task<IEnumerable<ParkingLot>> GetByOwnerIdAsync(Guid ownerId);

        Task<IEnumerable<ParkingLot>> GetWithCoverAsync();    

        Task<IEnumerable<ParkingLot>> GetWithPCDSpaceAsync();

        Task<ParkingLot> CreateAsync(ParkingLotDto dto);

        Task<ParkingLot> UpdateAsync([FromBody]ParkingLotDto dto, Guid id);

        Task<bool> DeleteAsync(Guid id);
    }
}
