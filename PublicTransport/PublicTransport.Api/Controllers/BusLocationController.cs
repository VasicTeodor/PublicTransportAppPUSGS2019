using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.SignalR;
using PublicTransport.Api.Data;
using PublicTransport.Api.Helpers;
using PublicTransport.Api.Hub;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusLocationController : Controller
    {
        private readonly IPublicTransportRepository _publicTransportRepository;
        private readonly BusLocationHub _locationHub;

        public BusLocationController(IPublicTransportRepository publicTransportRepository, BusLocationHub locationHub)
        {
            _publicTransportRepository = publicTransportRepository;
            _locationHub = locationHub;
        }

        // GET
        public async Task<IActionResult> Get(int lineId)
        {
            var locations = await GetLocationsForStations(lineId);

            if (locations.Count > 0)
            {
                _locationHub.StopTimer();
                _locationHub.InitializeHub(locations, lineId);
            }

            return Ok(new { Message = "Request Completed" });
        }

        private async Task<List<Location>> GetLocationsForStations(int lineId)
        {
            var allLocations = new List<Location>();

            var line = await _publicTransportRepository.GetLine(lineId);

            foreach (var station in line.Stations)
            {
                allLocations.Add(station.Station.Location);
            }

            return allLocations;
        }
    }
}