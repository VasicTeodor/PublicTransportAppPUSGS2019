﻿using System;
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
            return await _context.TimeTables.Include(t => t.Line).ThenInclude(s => s.Stations)
                                            .Include(t => t.Line).ThenInclude(b => b.Buses).ToListAsync();
        }
    }
}