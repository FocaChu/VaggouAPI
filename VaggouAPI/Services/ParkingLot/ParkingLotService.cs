using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IEnumerable<ParkingLot>> GetAllAsync()
        {
            return await IncludeAll().ToListAsync();
        }

        public async Task<ParkingLot?> GetByIdAsync(Guid id)
        {
            return await IncludeAll().FirstOrDefaultAsync(pl => pl.Id == id);
        }

        public async Task<IEnumerable<ParkingLot>> GetByAdressZipCodeAsync(string zipCode)
        {
            return await IncludeAll()
                .Where(pl => pl.Adress.ZipCode == zipCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<ParkingLot>> GetByProximityAsync(int latitude, int longitude, int raio)
        {
            return await IncludeAll()
                .Where(
                pl => pl.Adress.Longitude < (raio + longitude) &&
                      pl.Adress.Longitude > (longitude - raio) &&
                      pl.Adress.Latitude < (raio + latitude) &&
                      pl.Adress.Latitude > (latitude - raio)
                )
                .ToListAsync();
        }

        public async Task<IEnumerable<ParkingLot>> GetByOwnerIdAsync(Guid ownerId)
        {
            return await IncludeAll()
                .Where(pl => pl.OwnerId == ownerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ParkingLot>> GetWithCoverAsync()
        {
            return await IncludeAll()
                .Where(pl => pl.ParkingSpots.Any(ps => ps.IsCovered))
                .ToListAsync();
        }

        public async Task<IEnumerable<ParkingLot>> GetWithPCDSpaceAsync()
        {
            return await IncludeAll()
                .Where(pl => pl.ParkingSpots.Any(ps => ps.IsPCDSpace))
                .ToListAsync();
        }

        public async Task<ParkingLot> CreateAsync(ParkingLotDto dto)
        {
            var created = _mapper.Map<ParkingLot>(dto);
            await _context.ParkingLots.AddAsync(created);

            var adress = await _context.Adresses.FindAsync(dto.AdressId);
            if (adress == null)
                throw new BusinessException("Adress not found.");
            else
            {
                created.Adress = adress;
                created.AdressId = adress.Id;
            }

            var owner = await _context.Clients.FindAsync(dto.OwnerId);
            if (owner == null)
                throw new BusinessException("Owner not found.");
            else
            {
                created.Owner = owner;
                created.OwnerId = owner.Id;
            }
            await _context.SaveChangesAsync();
            return created;
        }

        public async Task<ParkingLot> UpdateAsync(ParkingLotDto dto, Guid id)
        {
            var updated = await _context.ParkingLots.FindAsync(id);
            if (updated == null) return null;

            _mapper.Map(dto, updated);

            var adress = await _context.Adresses.FindAsync(dto.AdressId);
            if (adress == null)
                throw new BusinessException("Adress not found.");
            else
            {
                updated.Adress = adress;
                updated.AdressId = adress.Id;
            }

            var owner = await _context.Clients.FindAsync(dto.OwnerId);
            if (owner == null)
                throw new BusinessException("Owner not found.");
            else
            {
                updated.Owner = owner;
                updated.OwnerId = owner.Id;
            }


            _context.ParkingLots.Update(updated);
            await _context.SaveChangesAsync();
            return updated;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.ParkingLots.FindAsync(id);
            if (entity == null) return false;

            _context.ParkingLots.Remove(entity);

            await _context.SaveChangesAsync();
            return true;
        }

        private IQueryable<ParkingLot> IncludeAll()
        {
            return _context.ParkingLots
                .Include(pl => pl.Adress)
                .Include(pl => pl.Owner)
                .Include(pl => pl.MonthlyReports)
                .Include(pl => pl.ParkingSpots)
                .Include(pl => pl.Favorites);
        }

    }
}
