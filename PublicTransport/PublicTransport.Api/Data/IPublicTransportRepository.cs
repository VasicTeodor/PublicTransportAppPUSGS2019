using System.Collections.Generic;
using System.Threading.Tasks;
using PublicTransport.Api.Dtos;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public interface IPublicTransportRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<PricelistItem>> GetPricelists(bool active, int userId);
        Task<IEnumerable<TimeTable>> GetTimetables(string type, string dayInWeek);
        Task<IEnumerable<User>> GetUsers(string accountStatus);
        Task<User> GetUser(int id);
        Task<bool> BuyTicketAsync(string ticketType, int userId = -1, string email = null);
        Task<bool> ValidateUserAccount(int userId, bool valid);
        Task<Ticket> ValidateUserTicket(int ticketId);
        Task<IEnumerable<Ticket>> GetTickets();
        Task<AllPricelistsForUsersDto> CalculateAllPricelists(List<PricelistItem> pricelist);
        Task<IEnumerable<Station>> GetStations();
        Task<Station> AddStation(Station station);
        Task<bool> RemoveStation(int stationId);
        Task<Station> UpdateStation(int stationId, NewStationDto station);
        Task<IEnumerable<Line>> GetLines();
        Task<Line> AddLine(Line line);
        Task<bool> RemoveLine(int lineId);
        Task<Line> UpdateLine(int lineId, NewLineDto line);
        Task<IEnumerable<TimeTable>> GetTimetableove();
        Task<TimeTable> AddTimetable(TimeTable timetable);
        Task<bool> RemoveTimetable(int timetableId);
        Task<TimeTable> UpdateTimetable(int timetableId, TimeTable timetable);
        Task<IEnumerable<PricelistItem>> GetPriceListove();
        Task<PricelistItem> AddPricelist(NewPricelistDto pricelist);
        Task<bool> RemovePricelist(int pricelistId);
        Task<PricelistItem> UpdatePricelist(int pricelistId, NewPricelistDto pricelist);
        Task<Station> GetStation(int stationId);
        Task<Line> GetLine(int lineId);
        Task<TimeTable> GetTimetable(int timetableId);
        Task<NewPricelistDto> GetPricelist(int pricelistId);
        Task AddLineToStation(int stationId, int lineId);
        Task AddStationToLine(int stationId, int lineId);
        Task<IEnumerable<Bus>> GetBuses();
        Task<Bus> AddBus(Bus bus);
        Task<bool> RemoveBus(int busId);
        Task<Bus> UpdateBus(int busId, Bus bus);
        Task<UserDiscount> GetDiscount(string type);
        Task<UserDiscount> UpdateDiscount(string type, UserDiscount discount);
    }
}