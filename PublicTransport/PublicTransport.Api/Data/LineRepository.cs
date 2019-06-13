using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public class LineRepository : ILineRepository
    {
        private readonly DataContext _context;

        public LineRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<Line> GetLine(int id)
        {
            return await _context.Lines.Include(s => s.Stations)
                .ThenInclude(sl => sl.Station).Include(b => b.Buses)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Line>> GetLines()
        {
            return await _context.Lines.Include(s => s.Stations).
                ThenInclude(sl => sl.Station)
                .Include(b => b.Buses).ToListAsync();
        }
    }
}
