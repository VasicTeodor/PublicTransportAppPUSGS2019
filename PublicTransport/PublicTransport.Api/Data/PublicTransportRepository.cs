using System.Threading.Tasks;

namespace PublicTransport.Api.Data
{
    public class PublicTransportRepository : IPublicTransportRepository
    {
        private readonly DataContext _context;

        public PublicTransportRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}