using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PublicTransport.Api.Dtos;
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
        private readonly IUserDiscountRepository _userDiscountRepository;
        private readonly UserManager<User> _userManager;

        public PublicTransportRepository(DataContext context, ITicketRepository ticketRepository,
            IPricelistItemRepository pricelistItemRepository, ITimeTableRepository timeTableRepository,
            IUserDiscountRepository userDiscountRepository, UserManager<User> userManager)
        {
            _context = context;
            _ticketRepository = ticketRepository;
            _pricelistItemRepository = pricelistItemRepository;
            _timeTableRepository = timeTableRepository;
            _userDiscountRepository = userDiscountRepository;
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
                //EmailService.SendEmail("You have successfuly bought hourly ticket.", email);
                return true;
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
                        PriceInfo = prInfo,
                        DateOfIssue = DateTime.Now
                    };

                    Add(newTicket);
                    return await SaveAll();
                }
            }

            return false;
        }

        public async Task<AllPricelistsForUsersDto> CalculateAllPricelists(List<PricelistItem> pricelist)
        {
            var regularDiscount = await _userDiscountRepository.GetUserDiscountForType("Regular");
            var studentDiscount = await _userDiscountRepository.GetUserDiscountForType("Student");
            var seniorDiscount = await _userDiscountRepository.GetUserDiscountForType("Senior");

            var allPricelists = new AllPricelistsForUsersDto()
            {
                SeniorPricelist = pricelist,
                RegularUserPricelist = pricelist,
                StudentPricelist = pricelist
            };

            foreach (var pricelistItem in allPricelists.SeniorPricelist)
            {
                pricelistItem.Price = pricelistItem.Price - (pricelistItem.Price / (decimal)seniorDiscount.Value);
            }

            foreach (var pricelistItem in allPricelists.StudentPricelist)
            {
                pricelistItem.Price = pricelistItem.Price - (pricelistItem.Price / (decimal)studentDiscount.Value);
            }

            foreach (var pricelistItem in allPricelists.RegularUserPricelist)
            {
                pricelistItem.Price = pricelistItem.Price - (pricelistItem.Price / (decimal)regularDiscount.Value);
            }

            return allPricelists;
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<PricelistItem>> GetPricelists(bool active, int userId)
        {
            var pricelist = await _pricelistItemRepository.GetPricelistItemsByActive(active);

            if (userId != -1)
            {
                var user = await _userManager.GetUserById(userId);

                var discount = await _userDiscountRepository.GetUserDiscountForType(user.UserType);

                foreach (var pricelistItem in pricelist)
                {
                    pricelistItem.Price = pricelistItem.Price - (pricelistItem.Price / (decimal)discount.Value);
                }
            }

            return pricelist;
        }

        public async Task<IEnumerable<Ticket>> GetTickets()
        {
            return await _ticketRepository.GetTickets();
        }

        public async Task<IEnumerable<TimeTable>> GetTimetables(string type, string dayInWeek)
        {
            return await _timeTableRepository.GetTimeTablesForParams(type, dayInWeek);
        }

        public async Task<User> GetUser(int id)
        {
            return await _userManager.GetUserById(id);
        }

        public async Task<IEnumerable<User>> GetUsers(string accountStatus)
        {
            if (accountStatus != null)
            {
                return await _userManager.Users.Include(u => u.Address)
                    .Include(u => u.Tickets)
                    .ThenInclude(t => t.PriceInfo)
                    .Where(u => u.AccountStatus == accountStatus)
                    .ToListAsync();
            }
            else
            {
                return await _userManager.Users.Include(u => u.Address)
                    .Include(u => u.Tickets)
                    .ThenInclude(t => t.PriceInfo)
                    .ToListAsync();
            }
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ValidateUserAccount(int userId, bool valid)
        {
            var user = await _userManager.GetUserById(userId);
            IdentityResult result = new IdentityResult();

            if (valid)
            {
                user.AccountStatus = "Active";
                user.Verified = true;

                result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    //EmailService.SendEmail(
                    //    "In the name of PublicTransport, I'm happy to inform you that your account is ACTIVATED.",
                    //    user.Email);
                }
            }
            else
            {
                user.AccountStatus = "Rejected";
                user.Verified = false;

                result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    //EmailService.SendEmail("In the name of PublicTransport, I'am sorry to inform you that your account document is REJECTED.", user.Email);
                }
            }


            return result.Succeeded;
        }

        public async Task<Ticket> ValidateUserTicket(int ticketId)
        {
            var ticket = await _ticketRepository.GetTicket(ticketId);

            if (ticket.TicketType == "Daily")
            {
                if (DateTime.Now.Date < ticket.DateOfIssue.AddDays(1).Date)
                {
                    ticket.IsValid = true;
                }
                else
                {
                    ticket.IsValid = false;
                }
            }
            else if (ticket.TicketType == "Hourly")
            {
                if (DateTime.Now < ticket.DateOfIssue.AddHours(1))
                {
                    ticket.IsValid = true;
                }
                else
                {
                    ticket.IsValid = false;
                }
            }
            else if (ticket.TicketType == "Monthly")
            {
                if (DateTime.Now.Date < ticket.DateOfIssue.AddMonths(1).Date)
                {
                    ticket.IsValid = true;
                }
                else
                {
                    ticket.IsValid = false;
                }
            }
            else if (ticket.TicketType == "Annual")
            {
                if (DateTime.Now.Date < ticket.DateOfIssue.AddYears(1).Date)
                {
                    ticket.IsValid = true;
                }
                else
                {
                    ticket.IsValid = false;
                }
            }

            if (await SaveAll())
            {
                return ticket;
            }
            else
            {
                return null;
            }
        }
    }
}