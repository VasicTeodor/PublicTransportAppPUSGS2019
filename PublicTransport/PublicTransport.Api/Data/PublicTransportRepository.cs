using System.Collections.Generic;
using System.Threading.Tasks;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public class PublicTransportRepository : IPublicTransportRepository
    {
        private readonly DataContext _context;
        private readonly ITicketRepository _ticketRepository;
        private readonly IPricelistItemRepository _pricelistItemRepository;
        private readonly ITimeTableRepository _timeTableRepository;

        public PublicTransportRepository(DataContext context, ITicketRepository ticketRepository,
            IPricelistItemRepository pricelistItemRepository, ITimeTableRepository timeTableRepository)
        {
            _context = context;
            _ticketRepository = ticketRepository;
            _pricelistItemRepository = pricelistItemRepository;
            _timeTableRepository = timeTableRepository;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<PricelistItem>> GetPricelists(bool active)
        {
            return await _pricelistItemRepository.GetPricelistItemsByActive(active);
        }

        public async Task<IEnumerable<TimeTable>> GetTimetables(string type)
        {
            return await _timeTableRepository.GetTimeTables();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}