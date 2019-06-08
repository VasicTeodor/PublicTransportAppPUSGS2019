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
        Task<IEnumerable<TimeTable>> GetTimetables(string type, string dayInWeek);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<bool> BuyTicketAsync(string ticketType, int userId = -1, string email = null);
        Task ValidateUserAccount(int userId, bool valid);
        Task ValidateUserTicket(int ticketId, bool valid);
    }
}