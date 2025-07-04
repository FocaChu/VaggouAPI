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

        public async Task<IEnumerable<Favorite>> GetAllAsync() =>
            await IncludeAll().ToListAsync();

        public async Task<Favorite?> GetByIdAsync(Guid id) =>
            await IncludeAll().FirstOrDefaultAsync(pl => pl.Id == id)
                ?? throw new NotFoundException("Favorite model not found.");

        public async Task<IEnumerable<Favorite>> GetByClientIdAsync(Guid clientId) =>
            await IncludeAll().Where(f => f.ClientId == clientId)
                .ToListAsync();

        public async Task<Favorite> CreateAsync(FavoriteDto dto)
        {
            var client = await _context.Clients.FindAsync(dto.ClientId)
                ?? throw new NotFoundException("Client not found.");

            var parkingLot = await _context.ParkingLots.FindAsync(dto.ParkingLotId)
                ?? throw new NotFoundException("Parking lot not found.");

            var created = _mapper.Map<Favorite>(dto);
            created.Client = client;
            created.ParkingLot = parkingLot;

            await _context.Favorites.AddAsync(created);

            await _context.SaveChangesAsync();
            return created;
        }

        public async Task<Favorite?> UpdateAsync(FavoriteDto dto, Guid id)
        {
            var updated = await _context.Favorites.FindAsync(id)
                ?? throw new NotFoundException("Favorite parking lot not found.");

            var client = await _context.Clients.FindAsync(dto.ClientId)
                ?? throw new NotFoundException("Client not found.");

            var parkingLot = await _context.ParkingLots.FindAsync(dto.ParkingLotId)
                ?? throw new NotFoundException("Parking lot not found.");

            _mapper.Map(dto, updated);
            updated.Client = client;
            updated.ParkingLot = parkingLot;

            _context.Favorites.Update(updated);
            await _context.SaveChangesAsync();
            return updated;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.ParkingLots.FindAsync(id)
                ?? throw new NotFoundException("Favorite parking lot not found.");

            _context.ParkingLots.Remove(entity);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Favorite> IncludeAll() =>
            _context.Favorites
                .Include(f => f.ParkingLot);
    }
}
