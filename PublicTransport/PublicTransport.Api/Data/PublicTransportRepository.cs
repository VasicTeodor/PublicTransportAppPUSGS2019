using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IStationRepository _stationRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ILineRepository _lineRepository;

        public PublicTransportRepository(DataContext context, ITicketRepository ticketRepository,
            IPricelistItemRepository pricelistItemRepository, ITimeTableRepository timeTableRepository,
            IUserDiscountRepository userDiscountRepository, IStationRepository stationRepository,
            IMapper mapper, UserManager<User> userManager, ILineRepository lineRepository)
        {
            _context = context;
            _ticketRepository = ticketRepository;
            _pricelistItemRepository = pricelistItemRepository;
            _timeTableRepository = timeTableRepository;
            _userDiscountRepository = userDiscountRepository;
            _stationRepository = stationRepository;
            _mapper = mapper;
            _userManager = userManager;
            _lineRepository = lineRepository;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public async Task<Line> AddLine(Line line)
        {
            Add(line);

            if (await SaveAll())
            {
                return line;
            }
            else
            {
                return null;
            }
        }

        public async Task<PricelistItem> AddPricelist(PricelistItem pricelist)
        {
            Add(pricelist);

            if (await SaveAll())
            {
                return pricelist;
            }
            else
            {
                return null;
            }
        }

        public async Task<Station> AddStation(Station station)
        {
            Add(station);

            if (await SaveAll())
            {
                return station;
            }
            else
            {
                return null;
            }
        }

        public async Task<TimeTable> AddTimetable(TimeTable timetable)
        {
            Add(timetable);

            if (await SaveAll())
            {
                return timetable;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> BuyTicketAsync(string ticketType, int userId = -1, string email = null)
        {
            if (userId == -1 && email != null)
            {
                PricelistItem prInfo = await _pricelistItemRepository.GetPriceListItemForTicketType(ticketType);
                Ticket newTicket = new Ticket()
                {
                    IsValid = true,
                    TicketType = ticketType,
                    PriceInfo = prInfo,
                    DateOfIssue = DateTime.Now
                };

                Add(newTicket);
                var result = await SaveAll();

                if (result)
                {
                    EmailService.SendEmail("You have successfuly bought hourly ticket.", email);
                    return true;
                }

                return false;
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
                SeniorPricelist = new List<PricelistItem>(){},
                RegularUserPricelist = new List<PricelistItem>(){},
                StudentPricelist = new List<PricelistItem>() { }
            };

            if (seniorDiscount.Value != 0)
            {
                foreach (var pricelistItem in pricelist)
                {
                    allPricelists.SeniorPricelist.Add(new PricelistItem()
                    {
                        Price = pricelistItem.Price - (pricelistItem.Price * ((decimal)seniorDiscount.Value / 100))
                    }); 
                }
            }
            else
            {
                allPricelists.SeniorPricelist.AddRange(pricelist);
            }

            if (studentDiscount.Value != 0)
            {
                foreach (var pricelistItem in pricelist)
                {
                    allPricelists.StudentPricelist.Add(new PricelistItem()
                    {
                        Price = pricelistItem.Price - (pricelistItem.Price * ((decimal) studentDiscount.Value / 100))
                    });
                }
            }
            else
            {
                allPricelists.StudentPricelist.AddRange(pricelist);
            }

            if (regularDiscount.Value != 0)
            {
                foreach (var pricelistItem in pricelist)
                {
                    allPricelists.RegularUserPricelist.Add(new PricelistItem()
                    {
                        Price = pricelistItem.Price - (pricelistItem.Price * ((decimal)regularDiscount.Value / 100))
                    }); 
                }
            }
            else
            {
                allPricelists.RegularUserPricelist.AddRange(pricelist);
            }

            return allPricelists;
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<Line>> GetLines()
        {
            return await _lineRepository.GetLines();
        }

        public async Task<IEnumerable<PricelistItem>> GetPriceListove()
        {
            return await _pricelistItemRepository.GetPricelistItems();
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

        public async Task<IEnumerable<Station>> GetStations()
        {
            return await _stationRepository.GetStations();
        }

        public async Task<IEnumerable<Ticket>> GetTickets()
        {
            return await _ticketRepository.GetTickets();
        }

        public async Task<IEnumerable<TimeTable>> GetTimetableove()
        {
            return await _timeTableRepository.GetTimeTables();
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

        public async Task<bool> RemoveLine(int lineId)
        {
            var line = await _lineRepository.GetLine(lineId);

            if (line != null)
            {
                Delete(line);

                if (await SaveAll())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> RemovePricelist(int pricelistId)
        {
            var pricelist = await _pricelistItemRepository.GetPriceListItem(pricelistId);

            if (pricelist != null)
            {
                Delete(pricelist);

                if (await SaveAll())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> RemoveStation(int stationId)
        {
            var station = await _stationRepository.GetStation(stationId);

            if (station != null)
            {
                Delete(station);

                if (await SaveAll())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> RemoveTimetable(int timetableId)
        {
            var timetable = await _timeTableRepository.GetTimeTable(timetableId);

            if (timetable != null)
            {
                Delete(timetable);

                if (await SaveAll())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Line> UpdateLine(int lineId, Line line)
        {
            var lineForUpdate = await _lineRepository.GetLine(lineId);

            _mapper.Map(line, lineForUpdate);

            if (await SaveAll())
            {
                return lineForUpdate;
            }
            else
            {
                return null;
            }
        }

        public async Task<PricelistItem> UpdatePricelist(int pricelistId, PricelistItem pricelist)
        {
            var pricelistForUpdate = await _pricelistItemRepository.GetPriceListItem(pricelistId);

            _mapper.Map(pricelist, pricelistForUpdate);

            if (await SaveAll())
            {
                return pricelistForUpdate;
            }
            else
            {
                return null;
            }
        }

        public async Task<Station> UpdateStation(int stationId, Station station)
        {
            var stationForUpdate = await _stationRepository.GetStation(stationId);

            _mapper.Map(station, stationForUpdate);

            if (await SaveAll())
            {
                return stationForUpdate;
            }
            else
            {
                return null;
            }
        }

        public async Task<TimeTable> UpdateTimetable(int timetableId, TimeTable timetable)
        {
            var timetableForUpdate = await _timeTableRepository.GetTimeTable(timetableId);

            _mapper.Map(timetable, timetableForUpdate);

            if (await SaveAll())
            {
                return timetableForUpdate;
            }
            else
            {
                return null;
            }
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
                if (ticket != null)
                {
                    return ticket;
                }
                return null;
            }
        }
    }
}