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

        public async Task<IEnumerable<ParkingSpot>> GetAllAsync() =>
            await _context.ParkingSpots.ToListAsync();

        public async Task<ParkingSpot?> GetByIdAsync(Guid id) =>
            await _context.ParkingSpots
                .FirstOrDefaultAsync(ps => ps.Id == id);

        public async Task<IEnumerable<ParkingSpot>> GetByParkingLotIdAsync(Guid parkingLotId) =>
            await _context.ParkingSpots
                .Where(ps => ps.ParkingLotId == parkingLotId)
                .ToListAsync();


        public async Task<ParkingSpot> CreateAsync(ParkingSpotDto dto)
        {
            var parkingLot = await _context.ParkingLots.FindAsync(dto.ParkingLotId)
                ?? throw new BusinessException("Estacionamento não encontrado.");

            var created = _mapper.Map<ParkingSpot>(dto);
            created.ParkingLot = parkingLot;

            await _context.ParkingSpots.AddAsync(created);
            await _context.SaveChangesAsync();
            return created;
        }

        public async Task<ParkingSpot?> UpdateAsync(ParkingSpotDto dto, Guid Id)
        {
            var updated = await IncludeAll()
                .FirstOrDefaultAsync(ps => ps.Id == Id)
                ?? throw new NotFoundException("Vaga não encontrada.");   

            var parkingLot = await _context.ParkingLots.FindAsync(dto.ParkingLotId)
                ?? throw new BusinessException("Estacionamento não encontrado.");

            _mapper.Map(dto, updated);
            updated.ParkingLot = parkingLot;

            _context.ParkingSpots.Update(updated);
            await _context.SaveChangesAsync();
            return updated;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.ParkingLots.FindAsync(id)
                ?? throw new NotFoundException("Vaga não encontrada para deleção.");

            _context.ParkingLots.Remove(entity);
            await _context.SaveChangesAsync();
        }

        private IQueryable<ParkingSpot> IncludeAll() =>
            _context.ParkingSpots
                .Include(pl => pl.ParkingLot);
    }
}
