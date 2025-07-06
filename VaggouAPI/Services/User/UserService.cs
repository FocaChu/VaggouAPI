using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class UserService : IUserService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public UserService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserSummaryDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.Client)
                .Include(u => u.Roles)
                .Select(u => new UserSummaryDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = u.Client.FullName,
                    Roles = u.Roles.Select(r => r.Name).ToList()
                })
                .OrderBy(u => u.FullName)
                .ToListAsync();

            return users;
        }

        public async Task<UserDetailDto> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Client)
                    .ThenInclude(c => c.OwnedParkingLots)
                .Include(u => u.Roles)
                .Where(u => u.Id == id)
                .Select(u => new UserDetailDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = u.Client.FullName,
                    Phone = u.Client.Phone,
                    Roles = u.Roles.Select(r => r.Name).ToList(),
                    OwnedParkingLotsCount = u.Client.OwnedParkingLots.Count
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new NotFoundException("Usuário não encontrado.");
            }

            return user;
        }

        public async Task<User?> GetByIdWithRolesAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task UpdateUserRolesAsync(Guid id, UpdateUserRolesDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new NotFoundException("Usuário não encontrado.");
            }

            var allRoles = await _context.Roles.ToListAsync();

            var desiredRoles = allRoles
                .Where(r => dto.RoleNames.Contains(r.Name, StringComparer.OrdinalIgnoreCase))
                .ToList();

            if (desiredRoles.Count != dto.RoleNames.Count)
            {
                var missingRoles = string.Join(", ", dto.RoleNames.Except(desiredRoles.Select(r => r.Name)));
                throw new BusinessException($"Os seguintes papéis não existem: {missingRoles}");
            }

            user.Roles = desiredRoles;

            await _context.SaveChangesAsync();
        }
    }
}
