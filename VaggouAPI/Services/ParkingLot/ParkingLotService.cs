using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class ParkingLotService : IParkingLotService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public ParkingLotService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParkingLot>> GetAllByScoreAsync() =>
            await IncludeAll().OrderByDescending(pl => pl.Score).ToListAsync();

        public async Task<IEnumerable<ParkingLot>> GetAllByProximityAsync(double latitude, double longitude)
        {
            return await IncludeAll()
                .OrderBy(pl =>
                    Math.Sqrt(
                        Math.Pow(pl.Address.Latitude - latitude, 2) +
                        Math.Pow(pl.Address.Longitude - longitude, 2)
                    )
                ).ToListAsync();
        }

        public async Task<ParkingLot?> GetByIdAsync(Guid id) =>
            await IncludeAll().FirstOrDefaultAsync(pl => pl.Id == id)
                ?? throw new NotFoundException("Parking lot not found.");

        public async Task<IEnumerable<ParkingLot>> GetByAdressZipCodeAsync(string zipCode) =>
            await IncludeAll().Where(pl => pl.Address.ZipCode == zipCode).ToListAsync();

        public async Task<IEnumerable<ParkingLot>> GetByProximityAsync(double latitude, double longitude, double raioKm) =>
            await IncludeAll()
                .Where(pl =>
                    Math.Sqrt(
                        Math.Pow(pl.Address.Latitude - latitude, 2) +
                        Math.Pow(pl.Address.Longitude - longitude, 2)
                    ) <= raioKm)
            .OrderByDescending(pl => pl.Score)
                .ToListAsync();

        public async Task<IEnumerable<ParkingLot>> GetByOwnerIdAsync(Guid ownerId) =>
            await IncludeAll().Where(pl => pl.OwnerId == ownerId).OrderByDescending(pl => pl.Score).ToListAsync();

        public async Task<IEnumerable<ParkingLot>> GetWithCoverAsync() =>
            await IncludeAll().Where(pl => pl.ParkingSpots.Any(ps => ps.IsCovered)).OrderByDescending(pl => pl.Score).ToListAsync();

        public async Task<IEnumerable<ParkingLot>> GetWithPCDSpaceAsync() =>
            await IncludeAll().Where(pl => pl.ParkingSpots.Any(ps => ps.IsPCDSpace)).OrderByDescending(pl => pl.Score).ToListAsync();

        public async Task<IEnumerable<ParkingLot>> GetMyParkingLotsAsync(Guid loggedInUserId)
        {
            return await IncludeAll()
                .Where(pl => pl.OwnerId == loggedInUserId)
                .OrderByDescending(pl => pl.Score)
                .ToListAsync();
        }

        public async Task<(ParkingLot parkingLot, User updatedUser, bool roleWasAdded)> 
        CreateAsync(ParkingLotDto dto, Guid loggedInUserId)
        {
            var owner = await _context.Clients.FindAsync(loggedInUserId)
                ?? throw new NotFoundException("Client not found.");

            var address = await _context.Adresses.FindAsync(dto.AddressId)
                ?? throw new NotFoundException("Address not found.");

            var entity = _mapper.Map<ParkingLot>(dto);
            entity.Address = address;
            entity.Owner = owner;
            entity.OwnerId = loggedInUserId;

            await _context.ParkingLots.AddAsync(entity);

            bool roleWasAdded = false;
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == loggedInUserId);

            if (user != null && !user.Roles.Any(r => r.Name == "ParkingLotOwner"))
            {
                var ownerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "ParkingLotOwner");
                if (ownerRole != null)
                {
                    user.Roles.Add(ownerRole);
                    roleWasAdded = true;
                }
            }

            await _context.SaveChangesAsync();

            return (entity, user, roleWasAdded);
        }

        public async Task<ParkingLot> UpdateAsync(ParkingLotDto dto, Guid parkingLotId, Guid loggedInUserId)
        {
            var entity = await _context.ParkingLots.FindAsync(parkingLotId)
                ?? throw new NotFoundException("Parking lot not found.");

            if (entity.OwnerId != loggedInUserId)
            {
                throw new UnauthorizedException("You do not have permission to modify this parking lot.");
            }

            var address = await _context.Adresses.FindAsync(dto.AddressId)
                ?? throw new NotFoundException("Address not found.");

            _mapper.Map(dto, entity);
            entity.Address = address;

            _context.ParkingLots.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(Guid parkingLotId, Guid loggedInUserId)
        {
            var entity = await _context.ParkingLots.FindAsync(parkingLotId)
                ?? throw new NotFoundException("Parking lot not found.");

            if (entity.OwnerId != loggedInUserId)
            {
                throw new UnauthorizedException("You do not have permission to delete this parking lot.");
            }

            _context.ParkingLots.Remove(entity);
            await _context.SaveChangesAsync();
        }

        private IQueryable<ParkingLot> IncludeAll() =>
            _context.ParkingLots
                .Include(pl => pl.Address)
                .Include(pl => pl.Owner)
                .Include(pl => pl.ParkingSpots);
    }
}