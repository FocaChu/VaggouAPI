using AutoMapper;

namespace VaggouAPI
{
    public class MonthlyReportService : IMonthlyReportService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public MonthlyReportService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
