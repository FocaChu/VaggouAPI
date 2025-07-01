using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class VehicleService : IVehicleService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public VehicleService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync() =>
            await IncludeAll().ToListAsync();

        public async Task<Vehicle> GetByIdAsync(Guid id) =>
            await IncludeAll().FirstOrDefaultAsync(v => v.Id == id)
                ?? throw new NotFoundException("Veículo não encontrado.");

        public async Task<IEnumerable<Vehicle>> GetByOwnerIdAsync(Guid ownerId) =>
            await IncludeAll().Where(v => v.OwnerId == ownerId).ToListAsync();

        public async Task<IEnumerable<Vehicle>> GetByModelIdAsync(Guid modelId) =>
            await IncludeAll().Where(v => v.VehicleModelId == modelId).ToListAsync();

        public async Task<IEnumerable<Vehicle>> GetPreRegisteredAsync() =>
            await IncludeAll().Where(v => v.IsPreRegistered).ToListAsync();

        public async Task<Vehicle> GetByLicensePlateAsync(string plate) =>
            await IncludeAll().FirstOrDefaultAsync(v => v.LicensePlate == plate)
                ?? throw new NotFoundException("Veículo com essa placa não encontrado.");

        public async Task<Vehicle> CreateAsync(VehicleDto dto)
        {
            var model = await _context.VehicleModels.FindAsync(dto.VehicleModelId)
                ?? throw new NotFoundException("Modelo de veículo não encontrado.");

            var owner = await _context.Clients.FindAsync(dto.OwnerId)
                ?? throw new NotFoundException("Proprietário não encontrado.");

            var entity = _mapper.Map<Vehicle>(dto);
            entity.VehicleModel = model;
            entity.Owner = owner;

            await _context.Vehicles.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Vehicle> UpdateAsync(VehicleDto dto, Guid id)
        {
            var entity = await _context.Vehicles.FindAsync(id)
                ?? throw new NotFoundException("Veículo não encontrado.");

            var model = await _context.VehicleModels.FindAsync(dto.VehicleModelId)
                ?? throw new NotFoundException("Modelo de veículo não encontrado.");

            var owner = await _context.Clients.FindAsync(dto.OwnerId)
                ?? throw new NotFoundException("Proprietário não encontrado.");

            _mapper.Map(dto, entity);
            entity.VehicleModel = model;
            entity.Owner = owner;

            _context.Vehicles.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Vehicles.FindAsync(id)
                ?? throw new NotFoundException("Veículo não encontrado para deleção.");

            _context.Vehicles.Remove(entity);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Vehicle> IncludeAll() =>
            _context.Vehicles
                .Include(v => v.VehicleModel)
                .Include(v => v.Owner)
                .Include(v => v.Reservations);
    }
}
