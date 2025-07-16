namespace VaggouAPI
{
    public interface IParkingLotService
    {
        Task<IEnumerable<ParkingLot>> SearchAsync(double? latitude, double? longitude, double? radiusKm, string? zipCode);
        
        Task<IEnumerable<ParkingLot>> GetAllSortedByScoreAsync();
        

        Task<ParkingLot> GetByIdAsync(Guid id); 

        Task<IEnumerable<ParkingLot>> GetByOwnerIdAsync(Guid ownerId);

        Task<IEnumerable<ParkingLot>> GetMyParkingLotsAsync(Guid loggedInUserId);


        Task<(ParkingLot parkingLot, User updatedUser, bool roleWasAdded)> CreateAsync(CreateParkingLotRequestDto dto, Guid loggedInUserId);
        
        Task<ParkingLot> UpdateAsync(Guid parkingLotId, UpdateParkingLotRequestDto dto, Guid loggedInUserId);

        Task DeleteAsync(Guid parkingLotId, Guid loggedInUserId);
    }
}