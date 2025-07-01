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

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await IncludeAll().ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(Guid id)
        {
            return await IncludeAll().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Reservation>> GetByClientIdAsync(Guid clientId)
        {
            return await IncludeAll()
                .Where(r => r.ClientId == clientId)
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
                throw new BusinessException("Parking spot is already reserved for the selected time.");

            var reservation = _mapper.Map<Reservation>(dto);

            // Valida e associa entidades relacionadas
            var client = await _context.Clients.FindAsync(dto.ClientId)
                ?? throw new BusinessException("Client not found.");

            var vehicle = await _context.Vehicles.FindAsync(dto.VehicleId)
                ?? throw new BusinessException("Vehicle not found.");

            var spot = await _context.ParkingSpots.FindAsync(dto.ParkingSpotId)
                ?? throw new BusinessException("Parking spot not found.");

            var payment = await _context.Payments.FindAsync(dto.PaymentId)
                ?? throw new BusinessException("Payment not found.");

            // Associação manual
            reservation.Client = client;
            reservation.ClientId = client.Id;

            reservation.Vehicle = vehicle;
            reservation.VehicleId = vehicle.Id;

            reservation.ParkingSpot = spot;
            reservation.ParkingSpotId = spot.Id;

            reservation.Payment = payment;
            reservation.PaymentId = payment.Id;

            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            return reservation;
        }

        public async Task<Reservation?> UpdateAsync(Guid id, ReservationDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return null;

            _mapper.Map(dto, reservation);

            // Atualiza relações se necessário
            var client = await _context.Clients.FindAsync(dto.ClientId)
                ?? throw new BusinessException("Client not found.");

            var vehicle = await _context.Vehicles.FindAsync(dto.VehicleId)
                ?? throw new BusinessException("Vehicle not found.");

            var spot = await _context.ParkingSpots.FindAsync(dto.ParkingSpotId)
                ?? throw new BusinessException("Parking spot not found.");

            var payment = await _context.Payments.FindAsync(dto.PaymentId)
                ?? throw new BusinessException("Payment not found.");

            reservation.Client = client;
            reservation.Vehicle = vehicle;
            reservation.ParkingSpot = spot;
            reservation.Payment = payment;

            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();

            return reservation;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return false;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return true;
        }

        private IQueryable<Reservation> IncludeAll()
        {
            return _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Vehicle)
                .Include(r => r.ParkingSpot)
                .Include(r => r.Payment);
        }
    }
}
