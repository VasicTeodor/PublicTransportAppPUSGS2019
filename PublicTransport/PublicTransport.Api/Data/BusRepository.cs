using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public class BusRepository : IBusRepository
    {
        private readonly DataContext _context;

        public BusRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<Bus> GetBus(int id)
        {
            return await _context.Busses.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Bus>> GetBuses()
        {
            return await _context.Busses.ToListAsync();
        }

        public async Task<IEnumerable<Bus>> GetFreeBuses(bool inUse)
        {
            return await _context.Busses.Where(b => b.InUse == false).ToListAsync();
        }
    }
}
