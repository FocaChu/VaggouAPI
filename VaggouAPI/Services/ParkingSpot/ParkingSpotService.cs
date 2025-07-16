using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class ParkingSpotService : IParkingSpotService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public ParkingSpotService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParkingSpot>> GetForParkingLotAsync(Guid parkingLotId)
        {
            var parkingLotExists = await _context.ParkingLots.AnyAsync(pl => pl.Id == parkingLotId);
            if (!parkingLotExists)
            {
                throw new NotFoundException("Parking lot not found.");
            }

            return await _context.ParkingSpots
                .Where(ps => ps.ParkingLotId == parkingLotId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ParkingSpot> GetByIdAsync(Guid spotId)
        {
            var spot = await _context.ParkingSpots
                .AsNoTracking()
                .FirstOrDefaultAsync(ps => ps.Id == spotId);

            return spot ?? throw new NotFoundException("Parking spot not found.");
        }

        public async Task<ParkingSpot> CreateAsync(Guid parkingLotId, CreateParkingSpotRequestDto dto, Guid loggedInUserId)
        {
            var parkingLot = await GetOwnedParkingLotAsync(parkingLotId, loggedInUserId);

            var spotIdentifierExists = await _context.ParkingSpots
                .AnyAsync(ps => ps.ParkingLotId == parkingLotId && ps.SpotIdentifier == dto.SpotIdentifier);

            if (spotIdentifierExists)
            {
                throw new BusinessException($"A spot with identifier '{dto.SpotIdentifier}' already exists in this parking lot.");
            }

            var spotEntity = _mapper.Map<ParkingSpot>(dto);
            spotEntity.ParkingLotId = parkingLot.Id; 

            await _context.ParkingSpots.AddAsync(spotEntity);
            await _context.SaveChangesAsync();

            return spotEntity;
        }

        public async Task<ParkingSpot> UpdateAsync(Guid spotId, UpdateParkingSpotRequestDto dto, Guid loggedInUserId)
        {
            var spotEntity = await _context.ParkingSpots
                .Include(ps => ps.ParkingLot)
                .FirstOrDefaultAsync(ps => ps.Id == spotId)
                    ?? throw new NotFoundException("Parking spot not found.");

            if (spotEntity.ParkingLot.OwnerId != loggedInUserId)
            {
                throw new UnauthorizedException("You do not have permission to modify this parking spot.");
            }

            var spotIdentifierExists = await _context.ParkingSpots
                .AnyAsync(ps => ps.ParkingLotId == spotEntity.ParkingLotId &&
                               ps.SpotIdentifier == dto.SpotIdentifier &&
                               ps.Id != spotId); 

            if (spotIdentifierExists)
            {
                throw new BusinessException($"A spot with identifier '{dto.SpotIdentifier}' already exists in this parking lot.");
            }

            _mapper.Map(dto, spotEntity);

            await _context.SaveChangesAsync();
            return spotEntity;
        }

        public async Task DeleteAsync(Guid spotId, Guid loggedInUserId)
        {
            var spotEntity = await _context.ParkingSpots
                .Include(ps => ps.ParkingLot)
                .FirstOrDefaultAsync(ps => ps.Id == spotId);

            if (spotEntity == null)
            {
                 return;
            }

            if (spotEntity.ParkingLot.OwnerId != loggedInUserId)
            {
                throw new UnauthorizedException("You do not have permission to delete this parking spot.");
            }

            _context.ParkingSpots.Remove(spotEntity);
            await _context.SaveChangesAsync();
        }

        private async Task<ParkingLot> GetOwnedParkingLotAsync(Guid parkingLotId, Guid ownerId)
        {
            var parkingLot = await _context.ParkingLots.FindAsync(parkingLotId)
                ?? throw new NotFoundException("Parking lot not found.");
            
            if (parkingLot.OwnerId != ownerId)
            {
                throw new UnauthorizedException("You do not have permission to manage this parking lot.");
            }

            return parkingLot;
        }
    }
}