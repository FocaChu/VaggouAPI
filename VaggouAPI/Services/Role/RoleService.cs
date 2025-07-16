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
            await _context.Roles.AsNoTracking().ToListAsync();

        public async Task<Role> GetByIdAsync(Guid id) =>
            await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new NotFoundException($"Role with Id: {id} not found");

        public async Task<Role> CreateAsync(CreateRoleRequestDto dto)
        {
            var nameExists = await _context.Roles.AnyAsync(r => r.Name == dto.Name);
            if (nameExists)
            {
                throw new BusinessException($"A role with the name '{dto.Name}' already exists.");
            }

            var entity = _mapper.Map<Role>(dto);
            await _context.Roles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var roleToDelete = await _context.Roles.Include(r => r.Users).FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new NotFoundException("Role not found.");

            if (roleToDelete.Users.Any())
            {
                throw new BusinessException("This role cannot be deleted as it is currently assigned to one or more users.");
            }

            var essentialRoles = new[] { "Admin", "ParkingLotOwner", "Consumer" };
            if (essentialRoles.Contains(roleToDelete.Name))
            {
                throw new BusinessException($"The essential role '{roleToDelete.Name}' cannot be deleted.");
            }

            _context.Roles.Remove(roleToDelete);
            await _context.SaveChangesAsync();
        }
    }
}