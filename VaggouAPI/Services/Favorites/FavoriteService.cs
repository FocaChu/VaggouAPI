using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IEnumerable<Favorite>> GetAllAsync()
        {
            return await IncludeAll().ToListAsync();
        }

        public async Task<Favorite?> GetByIdAsync(Guid id)
        {
            return await IncludeAll().FirstOrDefaultAsync(pl => pl.Id == id);
        }

        public async Task<IEnumerable<Favorite>> GetByClientIdAsync(Guid clientId)
        {
            return await IncludeAll().Where(f => f.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Favorite> CreateAsync(FavoriteDto dto)
        {
            var created = _mapper.Map<Favorite>(dto);

            var client = await _context.Clients.FindAsync(dto.ClientId);

            if(client == null)
                throw new BusinessException("Client not found.");
            else 
            {
                created.Client = client;
                created.ClientId = client.Id;
            }

            var parkingLot = await _context.ParkingLots.FindAsync(dto.ParkingLotId);

            if(parkingLot == null)
                throw new BusinessException("ParkingLot not found.");
            else 
            {
                created.ParkingLot = parkingLot;
                created.ParkingLotId = parkingLot.Id;
            }

            await _context.Favorites.AddAsync(created);

            await _context.SaveChangesAsync();
            return created;
        }

        public async Task<Favorite?> UpdateAsync(FavoriteDto dto, Guid id)
        {
            var updated = await _context.Favorites.FindAsync(id);

            if (updated == null) return null;

            _mapper.Map(dto, updated);

            var client = await _context.Clients.FindAsync(dto.ClientId);

            if (client == null)
                throw new BusinessException("Client not found.");
            else
            {
                updated.Client = client;
                updated.ClientId = client.Id;
            }

            var parkingLot = await _context.ParkingLots.FindAsync(dto.ParkingLotId);

            if (parkingLot == null)
                throw new BusinessException("ParkingLot not found.");
            else
            {
                updated.ParkingLot = parkingLot;
                updated.ParkingLotId = parkingLot.Id;
            }

            _context.Favorites.Update(updated);
            await _context.SaveChangesAsync();
            return updated;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Favorites.FindAsync(id);

            if (entity == null) 
                return false;

            _context.Remove(entity);

            await _context.SaveChangesAsync();
            return true;
        }

        private IQueryable<Favorite> IncludeAll()
        {
            return _context.Favorites
                .Include(f => f.ParkingLot);
        }
    }
}
