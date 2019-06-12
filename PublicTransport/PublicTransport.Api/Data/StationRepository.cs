using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public class StationRepository : IStationRepository
    {
        private readonly DataContext _context;

        public StationRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<Station> GetStation(int id)
        {
            return await _context.Stations.Include(s => s.StationLines).Include(s => s.Address).Include(s => s.Location).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Station>> GetStations()
        {
            return await _context.Stations.Include(s => s.StationLines).Include(s => s.Address).Include(s => s.Location).ToListAsync();
        }
    }
}
