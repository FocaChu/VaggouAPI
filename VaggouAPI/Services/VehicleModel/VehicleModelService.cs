using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public VehicleModelService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VehicleModel>> GetAllAsync() =>
            await _context.VehicleModels.Include(vm => vm.Vehicles).ToListAsync();

        public async Task<VehicleModel> GetByIdAsync(Guid id) =>
            await _context.VehicleModels.Include(vm => vm.Vehicles)
                .FirstOrDefaultAsync(vm => vm.Id == id)
                ?? throw new NotFoundException("Vehicle model not found.");

        public async Task<IEnumerable<VehicleModel>> GetByFuelTypeAsync(FuelType fuelType) =>
            await _context.VehicleModels
                .Include(vm => vm.Vehicles)
                .Where(vm => vm.FuelType == fuelType)
                .ToListAsync();

        public async Task<VehicleModel> CreateAsync(VehicleModelDto dto)
        {
            var entity = _mapper.Map<VehicleModel>(dto);

            await _context.VehicleModels.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<VehicleModel> UpdateAsync(VehicleModelDto dto, Guid id)
        {
            var entity = await _context.VehicleModels.FindAsync(id)
                ?? throw new NotFoundException("Vehicle model not found.");

            _mapper.Map(dto, entity);

            _context.VehicleModels.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.VehicleModels.FindAsync(id)
                ?? throw new NotFoundException("Vehicle model not found.");

            _context.VehicleModels.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
