using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class ReservationService : IReservationService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public ReservationService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync() =>
            await IncludeAll().ToListAsync();

        public async Task<Reservation?> GetByIdAsync(Guid id) =>
            await IncludeAll().FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new NotFoundException("Reservation not found.");

        public async Task<IEnumerable<Reservation>> GetByClientIdAsync(Guid clientId) =>
            await IncludeAll()
                .Where(r => r.ClientId == clientId)
                .ToListAsync();

        public async Task<IEnumerable<Reservation>> GetByMonthAsync(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            return await IncludeAll()
                .Where(p => p.Date >= startDate && p.Date < endDate)
                .ToListAsync();
        }

        public async Task<Reservation> CreateAsync(ReservationDto dto)
        {
            bool isOccupied = await _context.Reservations.AnyAsync(r =>
                r.ParkingSpotId == dto.ParkingSpotId &&
                r.Date.Date == dto.Date.Date &&
                r.Status != Status.Cancelled && 
                r.timeStart < dto.timeEnd &&    
                r.timeEnd > dto.timeStart       
            );

            if (isOccupied)
                throw new BusinessException("A vaga já está reservada no tempo selecionado.");

            var client = await _context.Clients.FindAsync(dto.ClientId)
                ?? throw new NotFoundException("Client not found.");

            var vehicle = await _context.Vehicles.FindAsync(dto.VehicleId)
                ?? throw new NotFoundException("Vehicle not found.");

            var spot = await _context.ParkingSpots.FindAsync(dto.ParkingSpotId)
                ?? throw new NotFoundException("Parking spot not found.");

            var payment = await _context.Payments.FindAsync(dto.PaymentId)
                ?? throw new NotFoundException("Payment not found.");

            var reservation = _mapper.Map<Reservation>(dto);
            reservation.Client = client;
            reservation.Vehicle = vehicle;
            reservation.ParkingSpot = spot;
            reservation.Payment = payment;

            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            return reservation;
        }

        public async Task<Reservation?> UpdateAsync(ReservationDto dto, Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id)
                ?? throw new NotFoundException("Reservation not found.");

            var client = await _context.Clients.FindAsync(dto.ClientId)
                ?? throw new NotFoundException("Client not found.");

            var vehicle = await _context.Vehicles.FindAsync(dto.VehicleId)
                ?? throw new NotFoundException("Vehicle not found.");

            var spot = await _context.ParkingSpots.FindAsync(dto.ParkingSpotId)
                ?? throw new NotFoundException("Parking spot not found.");

            var payment = await _context.Payments.FindAsync(dto.PaymentId)
                ?? throw new NotFoundException("Payment not found.");

            _mapper.Map(dto, reservation);
            reservation.Client = client;
            reservation.Vehicle = vehicle;
            reservation.ParkingSpot = spot;
            reservation.Payment = payment;

            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();

            return reservation;
        }

        public async Task DeleteAsync(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id)
                ?? throw new NotFoundException("Reservation not found.");

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Reservation> IncludeAll() =>
            _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Vehicle)
                .Include(r => r.ParkingSpot)
                .Include(r => r.Payment);
    }
}
