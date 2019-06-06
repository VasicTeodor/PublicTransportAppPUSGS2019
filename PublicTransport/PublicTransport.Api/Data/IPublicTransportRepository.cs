using System.Collections.Generic;
using System.Threading.Tasks;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public interface IPublicTransportRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<PricelistItem>> GetPricelists(bool active);
        Task<IEnumerable<TimeTable>> GetTimetables(string type);
    }
}