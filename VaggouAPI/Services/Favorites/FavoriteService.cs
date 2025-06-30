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
            return await _context.Favorites.ToListAsync();
        }

        public async Task<Favorite?> GetByIdAsync(Guid id)
        {
            return await _context.Favorites.FindAsync(id);
        }

        public async Task<IEnumerable<Favorite>> GetByClientIdAsync(Guid clientId)
        {
            return await _context.Favorites.Include(f => f.ParkingLot)
                .Where(f => f.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Favorite> CreateAsync([FromBody] FavoriteDto dto)
        {
            var entity = _mapper.Map<Favorite>(dto);

            await _context.Favorites.AddAsync(entity);

            var client = await _context.Clients.FindAsync(dto.ClientId);

            if(client == null)
                throw new BusinessException("Client not found.");
            else 
            { 
                entity.Client = client; 
                entity.ClientId = client.Id;
            }

            var parkingLot = await _context.ParkingLots.FindAsync(dto.ParkingLotId);

            if(parkingLot == null)
                throw new BusinessException("ParkingLot not found.");
            else 
            { 
                entity.ParkingLot = parkingLot; 
                entity.ParkingLotId = parkingLot.Id;
            }

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Favorite?> UpdateAsync([FromBody] FavoriteDto dto, Guid id)
        {
            var entity = await _context.Favorites.FindAsync(id);

            if (entity == null) return null;

            _mapper.Map(dto, entity);

            var client = await _context.Clients.FindAsync(dto.ClientId);

            if (client == null)
                throw new BusinessException("Client not found.");
            else
            {
                entity.Client = client;
                entity.ClientId = client.Id;
            }

            var parkingLot = await _context.ParkingLots.FindAsync(dto.ParkingLotId);

            if (parkingLot == null)
                throw new BusinessException("ParkingLot not found.");
            else
            {
                entity.ParkingLot = parkingLot;
                entity.ParkingLotId = parkingLot.Id;
            }

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Favorites.FindAsync(id);

            if (entity == null) return false;

            _context.Remove(entity);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
