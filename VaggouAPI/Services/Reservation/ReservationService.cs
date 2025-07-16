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

        public async Task<IEnumerable<Reservation>> GetMyReservationsAsync(Guid loggedInUserId)
        {
            return await _context.Reservations
                .Where(r => r.ClientId == loggedInUserId)
                .IncludeAllRelatedData()
                .AsNoTracking()
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }

        public async Task<Reservation> GetMyReservationByIdAsync(Guid reservationId, Guid loggedInUserId)
        {
            var reservation = await _context.Reservations
                .IncludeAllRelatedData()
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == reservationId)
                    ?? throw new NotFoundException("Reservation not found.");

            if (reservation.ClientId != loggedInUserId)
                throw new UnauthorizedException("You do not have permission to view this reservation.");

            return reservation;
        }

        public async Task<IEnumerable<Reservation>> GetForMyParkingLotAsync(Guid parkingLotId, Guid loggedInOwnerId)
        {
            var isOwner = await _context.ParkingLots.AnyAsync(pl => pl.Id == parkingLotId && pl.OwnerId == loggedInOwnerId);
            if (!isOwner)
            {
                throw new UnauthorizedException("You do not have permission to view reservations for this parking lot.");
            }

            return await _context.Reservations
                .Where(r => r.ParkingSpot.ParkingLotId == parkingLotId)
                .IncludeAllRelatedData()
                .AsNoTracking()
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }

        public async Task<Reservation> CreateAsync(CreateReservationRequestDto dto, Guid loggedInUserId)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == dto.VehicleId && v.OwnerId == loggedInUserId)
                ?? throw new BusinessException("Vehicle not found or does not belong to the user.");

            var spot = await _context.ParkingSpots.FindAsync(dto.ParkingSpotId)
                ?? throw new NotFoundException("Parking spot not found.");

            bool isOccupied = await _context.Reservations.AnyAsync(r =>
                r.ParkingSpotId == dto.ParkingSpotId &&
                r.Date.Date == dto.Date.Date &&
                r.Status != Status.Cancelled && r.Status != Status.Failed &&
                r.timeStart < dto.TimeEnd &&
                r.timeEnd > dto.TimeStart
            );
            if (isOccupied)
                throw new BusinessException("The parking spot is already reserved for the selected time.");

            var paymentEntity = new Payment
            {
                Status = Status.Pending, 
                Amount = 0, 
            };

            var reservationEntity = _mapper.Map<Reservation>(dto);
            reservationEntity.ClientId = loggedInUserId; 
            reservationEntity.Status = Status.Pending; 
            reservationEntity.Payment = paymentEntity;

            await _context.Reservations.AddAsync(reservationEntity);
            await _context.SaveChangesAsync();

            reservationEntity.Vehicle = vehicle;
            reservationEntity.ParkingSpot = spot;

            return reservationEntity;
        }

        public async Task<Reservation> CancelAsync(Guid reservationId, Guid loggedInUserId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Payment)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null)
                throw new NotFoundException("Reservation not found.");

            if (reservation.ClientId != loggedInUserId)
                throw new UnauthorizedException("You do not have permission to cancel this reservation.");

            var now = DateTime.UtcNow;
            var reservationStartDateTime = reservation.Date.Date + reservation.timeStart.ToTimeSpan();
            if (now >= reservationStartDateTime.AddHours(-1))
            {
                throw new BusinessException("Reservations can only be cancelled up to 1 hour before the start time.");
            }

            reservation.Status = Status.Cancelled;

            if (reservation.Payment != null)
            {
                reservation.Payment.Status = reservation.Payment.Status == Status.Success ? Status.Refunded : Status.Cancelled;
            }

            await _context.SaveChangesAsync();
            return reservation;
        }

    }

    public static class ReservationQueryExtensions
    {
        public static IQueryable<Reservation> IncludeAllRelatedData(this IQueryable<Reservation> query)
        {
            return query
                .Include(r => r.Client).ThenInclude(c => c.User)
                .Include(r => r.Vehicle).ThenInclude(v => v.VehicleModel)
                .Include(r => r.ParkingSpot)
                .Include(r => r.Payment).ThenInclude(p => p.PaymentMethod);
        }
    }
}