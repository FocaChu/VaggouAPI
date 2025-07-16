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

        public async Task<Client> GetMyProfileAsync(Guid loggedInUserId)
        {
            var client = await _context.Clients
                .Include(c => c.User) 
                .AsNoTracking() 
                .FirstOrDefaultAsync(c => c.Id == loggedInUserId)
                    ?? throw new NotFoundException("Client profile not found.");
            

            return client;
        }

        public async Task<Client> UpdateMyProfileAsync(Guid loggedInUserId, UpdateClientProfileRequestDto dto)
        {
            var clientEntity = await _context.Clients
                                     .Include(c => c.User) 
                                     .FirstOrDefaultAsync(c => c.Id == loggedInUserId)
                                     ?? throw new NotFoundException("Client profile not found.");
            

            _mapper.Map(dto, clientEntity);

            await _context.SaveChangesAsync();

            return clientEntity;
        }
    }
}