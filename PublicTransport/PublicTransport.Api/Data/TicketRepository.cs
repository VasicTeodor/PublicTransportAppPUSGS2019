using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public class TicketRepository : ITicketRepository
    {
        private readonly DataContext _context;

        public TicketRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Ticket>> GetTickets()
        {
            return await _context.Tickets.Include(t => t.User).ToListAsync();
        }

        public async Task<Ticket> GetTicket(int id)
        {
            return await _context.Tickets.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Ticket> BuyTicket(string type)
        {
            return await _context.Tickets.FirstOrDefaultAsync(t => t.TicketType == type);
        }
    }
}