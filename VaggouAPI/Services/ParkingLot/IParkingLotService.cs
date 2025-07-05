using Microsoft.AspNetCore.Mvc;

namespace VaggouAPI
{
    public interface IParkingLotService
    {
        Task<IEnumerable<ParkingLot>> GetAllByScoreAsync();

        Task<IEnumerable<ParkingLot>> GetAllByProximityAsync(double latitude, double longitude);

        Task<ParkingLot?> GetByIdAsync(Guid id);

        Task<IEnumerable<ParkingLot>> GetByAdressZipCodeAsync(string zipCode);

        Task<IEnumerable<ParkingLot>> GetByProximityAsync(double latitude, double longitude, double raioKm);

        Task<IEnumerable<ParkingLot>> GetByOwnerIdAsync(Guid ownerId);

        Task<IEnumerable<ParkingLot>> GetWithCoverAsync();    

        Task<IEnumerable<ParkingLot>> GetWithPCDSpaceAsync();

        Task<IEnumerable<ParkingLot>> GetMyParkingLotsAsync(Guid loggedInUserId);

        Task<ParkingLot> CreateAsync(ParkingLotDto dto, Guid loggedInUserId);

        Task<ParkingLot> UpdateAsync(ParkingLotDto dto, Guid parkingLotId, Guid loggedInUserId);

        Task DeleteAsync(Guid parkingLotId, Guid loggedInUserId);
    }
}
