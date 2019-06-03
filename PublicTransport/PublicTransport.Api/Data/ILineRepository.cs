using PublicTransport.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicTransport.Api.Data
{
    public interface ILineRepository
    {
        Task<IEnumerable<Line>> GetLines();
        Task<Line> GetLine(int id);
    }
}
