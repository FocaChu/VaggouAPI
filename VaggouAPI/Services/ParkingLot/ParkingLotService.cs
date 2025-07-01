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

        public async Task<IEnumerable<ParkingLot>> GetAllAsync() =>
            await IncludeAll().ToListAsync();

        public async Task<ParkingLot> GetByIdAsync(Guid id) =>
            await IncludeAll().FirstOrDefaultAsync(pl => pl.Id == id)
                ?? throw new NotFoundException("Estacionamento não encontrado.");

        public async Task<IEnumerable<ParkingLot>> GetByAdressZipCodeAsync(string zipCode) =>
            await IncludeAll().Where(pl => pl.Adress.ZipCode == zipCode).ToListAsync();

        public async Task<IEnumerable<ParkingLot>> GetByProximityAsync(double latitude, double longitude, double raioKm)
        {
            return await IncludeAll()
                .Where(pl =>
                    Math.Sqrt(
                        Math.Pow(pl.Adress.Latitude - latitude, 2) +
                        Math.Pow(pl.Adress.Longitude - longitude, 2)
                    ) <= raioKm)
                .ToListAsync();
        }


        public async Task<IEnumerable<ParkingLot>> GetByOwnerIdAsync(Guid ownerId) =>
            await IncludeAll().Where(pl => pl.OwnerId == ownerId).ToListAsync();

        public async Task<IEnumerable<ParkingLot>> GetWithCoverAsync() =>
            await IncludeAll().Where(pl => pl.ParkingSpots.Any(ps => ps.IsCovered)).ToListAsync();

        public async Task<IEnumerable<ParkingLot>> GetWithPCDSpaceAsync() =>
            await IncludeAll().Where(pl => pl.ParkingSpots.Any(ps => ps.IsPCDSpace)).ToListAsync();

        public async Task<ParkingLot> CreateAsync(ParkingLotDto dto)
        {
            var adress = await _context.Adresses.FindAsync(dto.AdressId)
                ?? throw new NotFoundException("Endereço não encontrado.");

            var owner = await _context.Clients.FindAsync(dto.OwnerId)
                ?? throw new NotFoundException("Dono não encontrado.");

            var entity = _mapper.Map<ParkingLot>(dto);
            entity.Adress = adress;
            entity.Owner = owner;

            await _context.ParkingLots.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ParkingLot> UpdateAsync(ParkingLotDto dto, Guid id)
        {
            var entity = await _context.ParkingLots.FindAsync(id)
                ?? throw new NotFoundException("Estacionamento não encontrado.");

            var adress = await _context.Adresses.FindAsync(dto.AdressId)
                ?? throw new NotFoundException("Endereço não encontrado.");

            var owner = await _context.Clients.FindAsync(dto.OwnerId)
                ?? throw new NotFoundException("Dono não encontrado.");

            _mapper.Map(dto, entity);
            entity.Adress = adress;
            entity.Owner = owner;

            _context.ParkingLots.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.ParkingLots.FindAsync(id)
                ?? throw new NotFoundException("Estacionamento não encontrado para deleção.");

            _context.ParkingLots.Remove(entity);
            await _context.SaveChangesAsync();
        }

        private IQueryable<ParkingLot> IncludeAll() =>
            _context.ParkingLots
                .Include(pl => pl.Adress)
                .Include(pl => pl.Owner)
                .Include(pl => pl.MonthlyReports)
                .Include(pl => pl.ParkingSpots)
                .Include(pl => pl.Favorites);
    }
}