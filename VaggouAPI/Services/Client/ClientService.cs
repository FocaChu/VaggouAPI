using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class ClientService : IClientService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public ClientService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ClientProfileDto> GetMyProfileAsync(Guid loggedInUserId)
        {
            var client = await _context.Clients
                .Include(c => c.User) 
                .Where(c => c.Id == loggedInUserId)
                .Select(c => new ClientProfileDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Email = c.User.Email,
                    Phone = c.Phone,
                    CPF = c.CPF
                })
                .FirstOrDefaultAsync();

            if (client == null)
            {
                throw new NotFoundException("Client profile not found.");
            }

            return client;
        }

        public async Task<ClientProfileDto> UpdateMyProfileAsync(Guid loggedInUserId, UpdateClientProfileDto dto)
        {
            var clientEntity = await _context.Clients.FindAsync(loggedInUserId);

            if (clientEntity == null)
            {
                throw new NotFoundException("Client profile not found.");
            }

            clientEntity.FullName = dto.FullName;
            clientEntity.Phone = dto.Phone;

            await _context.SaveChangesAsync();

            return await GetMyProfileAsync(loggedInUserId);
        }
    }
}
