using PublicTransport.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicTransport.Api.Data
{
    public interface ITimeTableRepository
    {
        Task<IEnumerable<TimeTable>> GetTimeTables();
        Task<IEnumerable<TimeTable>> GetTimeTablesForParams(string type, string day);
        Task<IEnumerable<Line>> GetLines(string type);
        Task<TimeTable> GetTimeTable(int id);
    }
}
