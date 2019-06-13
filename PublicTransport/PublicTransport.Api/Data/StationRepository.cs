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
            //return await (from s in _context.Stations
            //    join sl in _context.StationLines
            //        on s.Id equals sl.StationId
            //    join l in _context.Lines
            //        on sl.LineId equals l.Id
            //    where s.Type == type
            //    select new Line
            //    {
            //        Id = l.Id,
            //        LineNumber = l.LineNumber,
            //        Stations = l.Stations,
            //        Buses = l.Buses
            //    }).ToListAsync();
            return await _context.Stations.Include(s => s.StationLines).
                ThenInclude(sl => sl.Line).Include(s => s.Address)
                .Include(s => s.Location).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Station>> GetStations()
        {
            return await _context.Stations.Include(s => s.StationLines)
                .ThenInclude(sl => sl.Line).Include(s => s.Address)
                .Include(s => s.Location).ToListAsync();
        }
    }
}
