using System.Collections.Generic;
using System.Threading.Tasks;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetTickets();
        Task<Ticket> GetTicket(int id);
        Task<Ticket> BuyTicket(string type);
    }
}