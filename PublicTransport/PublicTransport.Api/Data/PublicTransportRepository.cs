using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var buses = new List<Bus>(line.Buses);

            line.Buses.Clear();

            Add(line);

            if (await SaveAll())
            {
                foreach (var bus in buses)
                {
                    var busToUpdate = await _context.Busses.FirstOrDefaultAsync(b => b.Id == bus.Id);
                    busToUpdate.InUse = true;
                    busToUpdate.LineId = line.Id;
                    _context.Update(busToUpdate);
                    line.Buses.Add(busToUpdate);

                    await SaveAll();
                }
                return line;
            }
            else
            {
                return null;
            }
        }

        public async Task AddLineToStation(int stationId, int lineId)
        {
            if (!(await _context.StationLines.AnyAsync(sl => sl.LineId == lineId && sl.StationId == stationId)))
            {
                var statLine = new StationLine()
                {
                    LineId = lineId,
                    StationId = stationId
                };

                Add(statLine);
                await SaveAll();
            }
        }

        public async Task<PricelistItem> AddPricelist(NewPricelistDto pricelist)
        {
            var prl = new Pricelist()
            {
                From = pricelist.From,
                To = pricelist.To,
                Active = pricelist.Active
            };

            _context.Pricelists.Add(prl);
            await SaveAll();


            var hourItem = await _context.Items.FirstOrDefaultAsync(i => i.Type == "Hourly");
            var dayItem = await _context.Items.FirstOrDefaultAsync(i => i.Type == "Daily");
            var monthItem = await _context.Items.FirstOrDefaultAsync(i => i.Type == "Monthly");
            var annualItem = await _context.Items.FirstOrDefaultAsync(i => i.Type == "Annual");
            var priceHourly = new PricelistItem();
            var priceDaily = new PricelistItem();
            var priceMonthly = new PricelistItem();
            var priceAnnual = new PricelistItem();

            _mapper.Map(pricelist, priceHourly);
            _mapper.Map(pricelist, priceDaily);
            _mapper.Map(pricelist, priceMonthly);
            _mapper.Map(pricelist, priceAnnual);

            priceHourly.Item = hourItem;
            priceHourly.Price = pricelist.PriceHourly;
            priceHourly.Pricelist = prl;

            priceDaily.Item = dayItem;
            priceDaily.Price = pricelist.PriceDaily;
            priceDaily.Pricelist = prl;

            priceMonthly.Item = monthItem;
            priceMonthly.Price = pricelist.PriceMonthly;
            priceMonthly.Pricelist = prl;

            priceAnnual.Item = annualItem;
            priceAnnual.Price = pricelist.PriceAnnual;
            priceAnnual.Pricelist = prl;

            if (pricelist.Active)
            {
                var pricelistFromDb = await _context.PricelistItems.Include(pr => pr.Item).Include(pr => pr.Pricelist)
                .Where(pr => pr.Pricelist.Active == true).ToListAsync();

                foreach (var pr in pricelistFromDb)
                {
                    pr.Pricelist.Active = false;
                }
            }

            _context.PricelistItems.Add(priceHourly);
            _context.PricelistItems.Add(priceDaily);
            _context.PricelistItems.Add(priceMonthly);
            _context.PricelistItems.Add(priceAnnual);

            if (await SaveAll())
            {
                return priceAnnual;
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

        public async Task AddStationToLine(int stationId, int lineId)
        {
            if (!(await _context.StationLines.AnyAsync(sl => sl.LineId == lineId && sl.StationId == stationId)))
            {
                var statLine = new StationLine()
                {
                    LineId = lineId,
                    StationId = stationId
                };

                Add(statLine);
                await SaveAll();
            }
        }

        public async Task<TimeTable> AddTimetable(TimeTable timetable)
        {
            var lineId = timetable.Line.Id;
            timetable.Line = null;
            Add(timetable);
            if (await SaveAll())
            {
                if (timetable.LineId == 0)
                {
                    var line = await _lineRepository.GetLine(lineId);
                    line.TimetableId = timetable.Id;
                    timetable.LineId = line.Id;
                    await SaveAll();
                }
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

        public async Task<Line> GetLine(int lineId)
        {
            return await _lineRepository.GetLine(lineId);
        }

        public async Task<IEnumerable<Line>> GetLines()
        {
            return await _lineRepository.GetLines();
        }

        public async Task<NewPricelistDto> GetPricelist(int pricelistId)
        {
            var priceHourly = await _context.PricelistItems.Include(pr => pr.Item).Include(pr => pr.Pricelist).FirstOrDefaultAsync(pr => pr.Item.Type == "Hourly" && pr.Pricelist.Active);
            var priceDaily = await _context.PricelistItems.Include(pr => pr.Item).Include(pr => pr.Pricelist).FirstOrDefaultAsync(pr => pr.Item.Type == "Daily" && pr.Pricelist.Active);
            var priceMonthly = await _context.PricelistItems.Include(pr => pr.Item).Include(pr => pr.Pricelist).FirstOrDefaultAsync(pr => pr.Item.Type == "Monthly" && pr.Pricelist.Active);
            var priceAnnual = await _context.PricelistItems.Include(pr => pr.Item).Include(pr => pr.Pricelist).FirstOrDefaultAsync(pr => pr.Item.Type == "Annual" && pr.Pricelist.Active);

            var prToRet = new NewPricelistDto
            {
                Active = true,
                From = priceHourly.Pricelist.From,
                To = priceHourly.Pricelist.To,
                IdHourly = priceHourly.Id,
                IdDaily = priceDaily.Id,
                IdMonthly = priceMonthly.Id,
                IdAnnual = priceAnnual.Id,
                PriceHourly = priceHourly.Price,
                PriceDaily = priceDaily.Price,
                PriceMonthly = priceMonthly.Price,
                PriceAnnual = priceAnnual.Price
            };

            return prToRet;
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

        public async Task<Station> GetStation(int stationId)
        {
            return await _stationRepository.GetStation(stationId);
        }

        public async Task<IEnumerable<Station>> GetStations()
        {
            return await _stationRepository.GetStations();
        }

        public async Task<IEnumerable<Ticket>> GetTickets()
        {
            return await _ticketRepository.GetTickets();
        }

        public async Task<TimeTable> GetTimetable(int timetableId)
        {
            return await _timeTableRepository.GetTimeTable(timetableId);
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
                var buses = new List<Bus>(line.Buses);
                
                Delete(line);

                if (await SaveAll())
                {
                    foreach (Bus bus in buses)
                    {
                        var busForUpdate = await _context.Busses.FirstOrDefaultAsync(b => b.Id == bus.Id);
                        busForUpdate.InUse = false;
                        _context.Update(busForUpdate);
                        await SaveAll();
                    }
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
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException e)
            {
                Trace.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<Line> UpdateLine(int lineId, NewLineDto line)
        {
            var lineForUpdate = await _lineRepository.GetLine(lineId);

            var busesInDb = new List<Bus>(lineForUpdate.Buses);

            _mapper.Map(line, lineForUpdate);

            var busesToUpdate = new List<Bus>(lineForUpdate.Buses);

            lineForUpdate.Buses.Clear();

            lineForUpdate.Stations = null;

            if (await SaveAll())
            {
                lineForUpdate = await _lineRepository.GetLine(lineId);

                foreach (var bus in busesToUpdate)
                {
                    busesInDb.Remove(busesInDb.FirstOrDefault(b => b.Id == bus.Id));
                }

                foreach (var bus in busesInDb)
                {
                    var busToUpdate = await _context.Busses.FirstOrDefaultAsync(b => b.Id == bus.Id);
                    busToUpdate.InUse = false;
                    lineForUpdate.Buses.Remove(busToUpdate);
                    _context.Update(busToUpdate);

                    await SaveAll();
                }

                foreach (var bus in busesToUpdate)
                {
                    var busToUpdate = await _context.Busses.FirstOrDefaultAsync(b => b.Id == bus.Id);
                    busToUpdate.InUse = true;
                    busToUpdate.LineId = lineForUpdate.Id;
                    _context.Update(busToUpdate);
                    lineForUpdate.Buses.Add(busToUpdate);

                    await SaveAll();
                }

                if (lineForUpdate.Stations == null)
                {
                    foreach (var station in line.Stations)
                    {
                        await AddStationToLine(station.Id, lineId);
                    }
                }
                return lineForUpdate;
            }
            else
            {
                return null;
            }
        }

        public async Task<PricelistItem> UpdatePricelist(int pricelistId, NewPricelistDto pricelist)
        {
            var priceHourly = await _context.PricelistItems.Include(pr => pr.Item).Include(pr => pr.Pricelist).FirstOrDefaultAsync(pr => pr.Item.Type == "Hourly" && pr.Id == pricelist.IdHourly);

            priceHourly.Pricelist.From = pricelist.From;
            priceHourly.Pricelist.To = pricelist.To;
            priceHourly.Pricelist.Active = pricelist.Active;

            _context.Update(priceHourly);
            await SaveAll();

            var priceDaily = await _context.PricelistItems.Include(pr => pr.Item).Include(pr => pr.Pricelist).FirstOrDefaultAsync(pr => pr.Item.Type == "Daily" && pr.Id == pricelist.IdDaily);
            var priceMonthly = await _context.PricelistItems.Include(pr => pr.Item).Include(pr => pr.Pricelist).FirstOrDefaultAsync(pr => pr.Item.Type == "Monthly" && pr.Id == pricelist.IdMonthly);
            var priceAnnual = await _context.PricelistItems.Include(pr => pr.Item).Include(pr => pr.Pricelist).FirstOrDefaultAsync(pr => pr.Item.Type == "Annual" && pr.Id == pricelist.IdAnnual);

            priceHourly.Price = pricelist.PriceHourly;
            priceDaily.Price = pricelist.PriceDaily;
            priceMonthly.Price = pricelist.PriceMonthly;
            priceAnnual.Price = pricelist.PriceAnnual;

            _context.Update(priceHourly);
            _context.Update(priceDaily);
            _context.Update(priceMonthly);
            _context.Update(priceAnnual);

            if (pricelist.Active)
            {
                var pricelistFromDb = await _context.PricelistItems.Include(pr => pr.Item).Include(pr => pr.Pricelist)
                .Where(pr => pr.Pricelist.Active == true && priceHourly.Pricelist.Id != pr.Pricelist.Id).ToListAsync();

                foreach (var pr in pricelistFromDb)
                {
                    pr.Pricelist.Active = false;
                }
            }

            if (await SaveAll())
            {
                return priceHourly;
            }
            else
            {
                return null;
            }
        }

        public async Task<Station> UpdateStation(int stationId, NewStationDto station)
        {
            var stationForUpdate = await _stationRepository.GetStation(stationId);

            _mapper.Map(station, stationForUpdate);

            stationForUpdate.StationLines = null;

            if (await SaveAll())
            {
                stationForUpdate = await _stationRepository.GetStation(stationId);
                if (stationForUpdate.StationLines == null)
                {
                    foreach (var lines in station.Lines)
                    {
                        await AddLineToStation(stationForUpdate.Id, lines.Id);
                    }
                }
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
            var lineId = timetableForUpdate.Line.Id;
            timetableForUpdate.Line = null;
            if (await SaveAll())
            {
                //var line = await _context.Lines.FirstOrDefaultAsync(l => l.Id == lineId);
                var line = await _lineRepository.GetLine(lineId);
                line.TimetableId = timetable.Id;
                timetableForUpdate.LineId = line.Id;
                await SaveAll();
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

        public async Task<IEnumerable<Bus>> GetBuses()
        {
            return await _context.Busses.ToListAsync();
        }

        public async Task<Bus> AddBus(Bus bus)
        {
            Add(bus);

            if (await SaveAll())
            {
                return bus;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> RemoveBus(int busId)
        {
            var busForDelete = await _context.Busses.FirstOrDefaultAsync(b => b.Id == busId);

            Delete(busForDelete);

            return await SaveAll();
        }
        public async Task<Bus> UpdateBus(int busId, Bus bus)
        {
            var busForUpdate = await _context.Busses.FirstOrDefaultAsync(b => b.Id == busId);

            _mapper.Map(bus, busForUpdate);

            if (await SaveAll())
            {
                return busForUpdate;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDiscount> GetDiscount(string type)
        {
            var result = await _userDiscountRepository.GetUserDiscountForType(type);

            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDiscount> UpdateDiscount(string type, UserDiscount discount)
        {
            var discountForUpdate = await _userDiscountRepository.GetUserDiscountForType(type);

            _context.Update(discountForUpdate);

            discountForUpdate.Value = discount.Value;

            if (await SaveAll())
            {
                return discountForUpdate;
            }
            else
            {
                return null;
            }
        }
    }
}