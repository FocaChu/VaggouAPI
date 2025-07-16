using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class FavoriteService : IFavoriteService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public FavoriteService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Favorite>> GetMyFavoritesAsync(Guid loggedInUserId)
        {
            return await _context.Favorites
                .Include(f => f.ParkingLot)
                    .ThenInclude(pl => pl.Address) 
                .Include(f => f.ParkingLot)
                    .ThenInclude(pl => pl.Images)
                .Where(f => f.ClientId == loggedInUserId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Favorite> CreateAsync(Guid loggedInUserId, CreateFavoriteRequestDto dto)
        {
            var parkingLotExists = await _context.ParkingLots.AnyAsync(pl => pl.Id == dto.ParkingLotId);
            if (!parkingLotExists)
            {
                throw new NotFoundException("Parking lot not found.");
            }

            var alreadyFavorite = await _context.Favorites.AnyAsync(f =>
                f.ClientId == loggedInUserId &&
                f.ParkingLotId == dto.ParkingLotId);

            if (alreadyFavorite)
            {
                throw new BusinessException("This parking lot is already in your favorites.");
            }

            var favoriteEntity = new Favorite
            {
                ClientId = loggedInUserId,
                ParkingLotId = dto.ParkingLotId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Favorites.AddAsync(favoriteEntity);
            await _context.SaveChangesAsync();

            await _context.Entry(favoriteEntity)
                          .Reference(f => f.ParkingLot)
                          .LoadAsync();

            return favoriteEntity;
        }

        public async Task DeleteAsync(Guid favoriteId, Guid loggedInUserId)
        {
            var favoriteEntity = await _context.Favorites.FindAsync(favoriteId)
                ?? throw new NotFoundException("Favorite not found.");
            
            if (favoriteEntity.ClientId != loggedInUserId)
            {
                throw new UnauthorizedException("You do not have permission to delete this favorite.");
            }

            _context.Favorites.Remove(favoriteEntity);
            await _context.SaveChangesAsync();
        }
    }
}