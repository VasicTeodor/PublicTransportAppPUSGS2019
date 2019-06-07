using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PublicTransport.Api.Helpers;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public class PublicTransportRepository : IPublicTransportRepository
    {
        private readonly DataContext _context;
        private readonly ITicketRepository _ticketRepository;
        private readonly IPricelistItemRepository _pricelistItemRepository;
        private readonly ITimeTableRepository _timeTableRepository;
        private readonly UserManager<User> _userManager;

        public PublicTransportRepository(DataContext context, ITicketRepository ticketRepository,
            IPricelistItemRepository pricelistItemRepository, ITimeTableRepository timeTableRepository, UserManager<User> userManager)
        {
            _context = context;
            _ticketRepository = ticketRepository;
            _pricelistItemRepository = pricelistItemRepository;
            _timeTableRepository = timeTableRepository;
            _userManager = userManager;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public async Task<bool> BuyTicketAsync(string ticketType, int userId = -1, string email = null)
        {
            if (userId == -1 && email != null)
            {
                var smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com", // set your SMTP server name here
                    Port = 587, // Port 
                    EnableSsl = true,
                    Credentials = new NetworkCredential("from@gmail.com", "blabla")
                };

                using (var message = new MailMessage("from@gmail.com", email)
                {
                    Subject = "Subject",
                    Body = "Body"
                })
                {
                    await smtpClient.SendMailAsync(message);
                    return true;
                }
            }

            if (userId != -1)
            {
                var userFromDatabase = await _userManager.GetUserById(userId);
                if (userFromDatabase.Verified == true)
                {
                    PricelistItem prInfo = await _pricelistItemRepository.GetPriceListItemForTicketType(ticketType);
                    Ticket newTicket = new Ticket()
                    {
                        User = userFromDatabase,
                        IsValid = true,
                        TicketType = ticketType,
                        PriceInfo = prInfo
                    };

                    Add(newTicket);
                    return await SaveAll();
                }
            }

            return false;
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<PricelistItem>> GetPricelists(bool active)
        {
            return await _pricelistItemRepository.GetPricelistItemsByActive(active);
        }

        public async Task<IEnumerable<TimeTable>> GetTimetables(string type, string dayInWeek)
        {
            return await _timeTableRepository.GetTimeTablesForParams(type, dayInWeek);
        }

        public async Task<User> GetUser(int id)
        {
            return await _userManager.GetUserById(id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userManager.Users.Include(u => u.Address)
                .Include(u => u.Tickets)
                .ThenInclude(t => t.PriceInfo)
                .ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}