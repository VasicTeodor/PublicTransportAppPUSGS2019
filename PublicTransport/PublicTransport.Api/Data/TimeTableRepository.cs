using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public class TimeTableRepository : ITimeTableRepository
    {
        private readonly DataContext _context;

        public TimeTableRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Line>> GetLines(string type)
        {
            return await (from t in _context.TimeTables
                            join l in _context.Lines
                            on t.Line.Id equals l.Id
                            where t.Type == type
                            select new Line
                            {
                                Id = l.Id,
                                LineNumber = l.LineNumber,
                                Stations = l.Stations,
                                Buses = l.Buses
                            }).ToListAsync();
        }

        public async Task<TimeTable> GetTimeTable(int id)
        {
            return await _context.TimeTables.Include(t => t.Line).ThenInclude(s => s.Stations)
                                            .Include(t => t.Line).ThenInclude(b => b.Buses).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TimeTable>> GetTimeTables()
        {
            //return await (from t in _context.TimeTables
            //              join l in _context.Lines
            //              on t.Id equals l.TimetableId
            //              join s in _context.StationLines
            //              on l.Id equals s.LineId
            //              select new TimeTable
            //              {
            //                  Id = t.Id,
            //                  Type = t.Type,
            //                  Day = t.Day,
            //                  Departures = t.Departures,
            //                  Line = new Line
            //                  {
            //                      Id = l.Id,
            //                      Name = l.Name,
            //                      LineNumber = l.LineNumber,
            //                      TimetableId = t.Id,
            //                      Stations = new List<StationLine>
            //                      {
            //                          new StationLine {Line = l, LineId = l.Id, Station = s.Station, StationId = s.Station.Id}
            //                      }
            //                  }
            //              }).ToListAsync();

            return await _context.TimeTables.Include(t => t.Line).ThenInclude(s => s.Stations)
                                            .Include(t => t.Line).ThenInclude(b => b.Buses).ToListAsync();
        }

        public async Task<IEnumerable<TimeTable>> GetTimeTablesForParams(string type, string day)
        {
            return await _context.TimeTables.Include(t => t.Line).ThenInclude(s => s.Stations)
                .Include(t => t.Line).ThenInclude(b => b.Buses).Where(t => t.Day == day && t.Type == type).ToListAsync();
        }
    }
}
