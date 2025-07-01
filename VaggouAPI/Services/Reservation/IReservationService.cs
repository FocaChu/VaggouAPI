namespace VaggouAPI
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetAllAsync();

        Task<Reservation?> GetByIdAsync(Guid id);

        Task<IEnumerable<Reservation>> GetByClientIdAsync(Guid clientId);

        Task<IEnumerable<Reservation>> GetByMonthAsync(int year, int month);

        Task<Reservation> CreateAsync(ReservationDto dto);

        Task<Reservation?> UpdateAsync(ReservationDto dto, Guid id);

        Task DeleteAsync(Guid id);
    }
}
