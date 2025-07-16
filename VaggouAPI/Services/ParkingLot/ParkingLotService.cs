using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace VaggouAPI
{
    public class ParkingLotService : IParkingLotService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;
        private const double EarthRadiusKm = 6371;

        public ParkingLotService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ParkingLot> GetByIdAsync(Guid id)
        {
            var parkingLot = await _context.ParkingLots
                .AsNoTracking()
                .Include(pl => pl.Address)
                .Include(pl => pl.Owner).ThenInclude(c => c.User) 
                .Include(pl => pl.ParkingSpots)
                .Include(pl => pl.Images)
                .FirstOrDefaultAsync(pl => pl.Id == id);

            return parkingLot ?? throw new NotFoundException("Parking lot not found.");
        }

        public async Task<IEnumerable<ParkingLot>> SearchAsync(double? latitude, double? longitude, double? radiusKm, string? zipCode)
        {
            IQueryable<ParkingLot> query = _context.ParkingLots
                .AsNoTracking()
                .Include(pl => pl.Address)
                .Include(pl => pl.Images);

            if (!string.IsNullOrWhiteSpace(zipCode))
            {
                query = query.Where(pl => pl.Address.ZipCode == zipCode);
            }

            if (latitude.HasValue && longitude.HasValue)
            {
                query = query.OrderBy(pl => Math.Sqrt(
                    Math.Pow(pl.Address.Latitude - latitude.Value, 2) +
                    Math.Pow(pl.Address.Longitude - longitude.Value, 2)
                ));

                if (radiusKm.HasValue)
                {
                    query = query.Where(pl => Math.Sqrt(
                        Math.Pow(pl.Address.Latitude - latitude.Value, 2) +
                        Math.Pow(pl.Address.Longitude - longitude.Value, 2)
                    ) <= radiusKm.Value);
                }
            }
            else
            {
                query = query.OrderByDescending(pl => pl.Score);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ParkingLot>> GetAllSortedByScoreAsync()
        {
            return await _context.ParkingLots
               .AsNoTracking()
               .Include(pl => pl.Address)
               .Include(pl => pl.Images)
               .OrderByDescending(pl => pl.Score)
               .ToListAsync();
        }

        public async Task<IEnumerable<ParkingLot>> GetByOwnerIdAsync(Guid ownerId)
        {
            return await _context.ParkingLots
                .AsNoTracking()
                .Where(pl => pl.OwnerId == ownerId)
                .Include(pl => pl.Address)
                .OrderByDescending(pl => pl.Score)
                .ToListAsync();
        }

        public async Task<IEnumerable<ParkingLot>> GetMyParkingLotsAsync(Guid loggedInUserId)
        {
            return await _context.ParkingLots
                .AsNoTracking()
                .Where(pl => pl.OwnerId == loggedInUserId)
                .Include(pl => pl.Address)
                .Include(pl => pl.Images)
                .OrderByDescending(pl => pl.Score)
                .ToListAsync();
        }

        public async Task<(ParkingLot parkingLot, User updatedUser, bool roleWasAdded)> CreateAsync(CreateParkingLotRequestDto dto, Guid loggedInUserId)
        {
            var owner = await _context.Clients.FindAsync(loggedInUserId)
                ?? throw new NotFoundException("Client profile not found.");

            var address = await _context.Adresses.FindAsync(dto.AddressId)
                ?? throw new NotFoundException("Address not found.");

            var parkingLotEntity = _mapper.Map<ParkingLot>(dto);
            parkingLotEntity.OwnerId = loggedInUserId;
            parkingLotEntity.AddressId = dto.AddressId;

            await _context.ParkingLots.AddAsync(parkingLotEntity);

            var user = await _context.Users.Include(u => u.Roles).FirstAsync(u => u.Id == loggedInUserId);
            bool roleWasAdded = await AddOwnerRoleToUserIfNeeded(user);

            await _context.SaveChangesAsync();

            parkingLotEntity.Owner = owner;
            parkingLotEntity.Address = address;

            return (parkingLotEntity, user, roleWasAdded);
        }

        public async Task<ParkingLot> UpdateAsync(Guid parkingLotId, UpdateParkingLotRequestDto dto, Guid loggedInUserId)
        {
            var entity = await _context.ParkingLots
                .Include(pl => pl.Address)
                .FirstOrDefaultAsync(pl => pl.Id == parkingLotId)
                    ?? throw new NotFoundException("Parking lot not found.");

            if (entity.OwnerId != loggedInUserId)
            {
                throw new UnauthorizedException("You do not have permission to modify this parking lot.");
            }

            if (entity.AddressId != dto.AddressId)
            {
                var newAddress = await _context.Adresses.FindAsync(dto.AddressId)
                    ?? throw new NotFoundException("The new address specified was not found.");
                entity.Address = newAddress;
            }

            _mapper.Map(dto, entity);

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

        private async Task<bool> AddOwnerRoleToUserIfNeeded(User user)
        {
            const string ownerRoleName = "ParkingLotOwner";
            if (!user.Roles.Any(r => r.Name == ownerRoleName))
            {
                var ownerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == ownerRoleName);
                if (ownerRole != null)
                {
                    user.Roles.Add(ownerRole);
                    return true;
                }
            }
            return false;
        }
    }
}