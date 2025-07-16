namespace VaggouAPI
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetMyReservationsAsync(Guid loggedInUserId);

        Task<Reservation> GetMyReservationByIdAsync(Guid reservationId, Guid loggedInUserId);

        Task<IEnumerable<Reservation>> GetForMyParkingLotAsync(Guid parkingLotId, Guid loggedInOwnerId);

        Task<Reservation> CreateAsync(CreateReservationRequestDto dto, Guid loggedInUserId);

        Task<Reservation> CancelAsync(Guid reservationId, Guid loggedInUserId);
    }
}