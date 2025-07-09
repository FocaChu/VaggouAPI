using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class RoleService : IRoleService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public RoleService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Role>> GetAllAsync() =>
            await _context.Roles
                .ToListAsync();

        public async Task<Role?> GetByIdAsync(Guid id) =>
            await _context.Roles.FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new NotFoundException($"Role with Id: {id} not found");

        public async Task<Role?> CreateAsync(RoleDto dto)
        {
            var created = _mapper.Map<Role>(dto);

            _context.Roles.Add(created);
            await _context.SaveChangesAsync();
            return created;
        }

        public async Task<Role?> UpdateAsync(RoleDto dto, Guid id)
        {
            var updated = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new NotFoundException("Role not found.");

            _mapper.Map(dto, updated);

            _context.Roles.Update(updated);
            await _context.SaveChangesAsync();
            return updated;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new NotFoundException("Role not found.");
            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
