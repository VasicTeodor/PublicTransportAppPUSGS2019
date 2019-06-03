using PublicTransport.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicTransport.Api.Data
{
    public interface IStationRepository
    {
        Task<IEnumerable<Station>> GetStations();
        Task<Station> GetStation(int id);
    }
}
