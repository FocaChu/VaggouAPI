namespace VaggouAPI
{
    public interface IParkingSpotService
    {
        Task<IEnumerable<ParkingSpot>> GetForParkingLotAsync(Guid parkingLotId);

        Task<ParkingSpot> GetByIdAsync(Guid spotId); 


        Task<ParkingSpot> CreateAsync(Guid parkingLotId, CreateParkingSpotRequestDto dto, Guid loggedInUserId);

        Task<ParkingSpot> UpdateAsync(Guid spotId, UpdateParkingSpotRequestDto dto, Guid loggedInUserId);

        Task DeleteAsync(Guid spotId, Guid loggedInUserId);
    }
}