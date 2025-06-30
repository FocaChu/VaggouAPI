using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IEnumerable<ParkingSpot>> GetAllAsync()
        {
            return await _context.ParkingSpots.ToListAsync();
        }

        public async Task<ParkingSpot?> GetByIdAsync(Guid id)
        {
            return await _context.ParkingSpots
                .FirstOrDefaultAsync(ps => ps.Id == id);
        }

        public async Task<IEnumerable<ParkingSpot>> GetByParkingLotIdAsync(Guid parkingLotId)
        {
            return await _context.ParkingSpots
                .Where(ps => ps.ParkingLotId == parkingLotId)
                .ToListAsync();
        }

        public async Task<ParkingSpot> CreateAsync(ParkingSpotDto dto)
        {
            var created = _mapper.Map<ParkingSpot>(dto);

            var parkingLot = await _context.ParkingLots.FindAsync(dto.ParkingLotId);
            if (parkingLot == null)
                    throw new BusinessException("ParkingLot not found.");
            else
            {
                created.ParkingLot = parkingLot;
                created.ParkingLotId = parkingLot.Id;
            }

            await _context.ParkingSpots.AddAsync(created);
            await _context.SaveChangesAsync();
            return created;
        }

        public async Task<ParkingSpot?> UpdateAsync(ParkingSpotDto dto, Guid Id)
        {
            var updated = await _context.ParkingSpots
                .Include(ps => ps.ParkingLot)
                .FirstOrDefaultAsync(ps => ps.Id == Id);

            if (updated == null) return null;

            _mapper.Map(dto, updated);

            var parkingLot = await _context.ParkingLots.FindAsync(dto.ParkingLotId);
            if (parkingLot == null)
                throw new BusinessException("ParkingLot not found.");
            else
            {
                updated.ParkingLot = parkingLot;
                updated.ParkingLotId = parkingLot.Id;
            }

            _context.ParkingSpots.Update(updated);
            await _context.SaveChangesAsync();
            return updated;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.ParkingSpots.FindAsync(id);

            if (entity == null)
                return false;

            _context.ParkingSpots.Remove(entity);
            _context.SaveChanges();

            return true;
        }
    }
}
