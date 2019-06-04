using PublicTransport.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicTransport.Api.Data
{
    public interface IBusRepository
    {
        Task<IEnumerable<Bus>> GetBuses();
        Task<Bus> GetBus(int id);
        Task<IEnumerable<Bus>> GetFreeBuses(bool inUse);
    }
}
