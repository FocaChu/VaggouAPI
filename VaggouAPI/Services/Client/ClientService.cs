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
    }
}
