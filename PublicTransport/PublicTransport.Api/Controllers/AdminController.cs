using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicTransport.Api.Data;
using PublicTransport.Api.Dtos;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPublicTransportRepository _publicTransportRepository;

        public AdminController(IMapper mapper, IPublicTransportRepository publicTransportRepository)
        {
            _mapper = mapper;
            _publicTransportRepository = publicTransportRepository;
        }

        [AllowAnonymous]
        [HttpGet("getStations")]
        public async Task<IActionResult> GetStations()
        {
            var stations = await _publicTransportRepository.GetStations();

            if (stations != null)
            {
                return Ok(stations);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("getBusses")]
        public async Task<IActionResult> GetBusses()
        {
            var busses = await _publicTransportRepository.GetBuses();

            if (busses != null)
            {
                return Ok(busses.Where(b => b.InUse == false));
            }
            else
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpGet("getLines")]
        public async Task<IActionResult> GetLines()
        {
            var lines = await _publicTransportRepository.GetLines();

            if (lines != null)
            {
                return Ok(lines);
            }
            else
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpGet("getTimetables")]
        public async Task<IActionResult> GetTimetables()
        {
            var timetables = await _publicTransportRepository.GetTimetableove();

            if (timetables != null)
            {
                return Ok(timetables);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("getAllPricelists")]
        public async Task<IActionResult> getAllPricelists()
        {
            var pricelists = await _publicTransportRepository.GetPriceListove();

            if (pricelists != null)
            {
                return Ok(pricelists);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("getStation")]
        public async Task<IActionResult> GetStation(int stationId)
        {
            var stations = await _publicTransportRepository.GetStation(stationId);

            if (stations != null)
            {
                return Ok(stations);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("getLine")]
        public async Task<IActionResult> GetLine(int lineId)
        {
            var lines = await _publicTransportRepository.GetLine(lineId);

            if (lines != null)
            {
                return Ok(lines);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("getTimetable")]
        public async Task<IActionResult> GetTimetable(int timetableId)
        {
            var timetables = await _publicTransportRepository.GetTimetable(timetableId);

            if (timetables != null)
            {
                return Ok(timetables);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("getPricelist")]
        public async Task<IActionResult> GetPricelist(int pricelistId)
        {
            var pricelists = await _publicTransportRepository.GetPricelist(pricelistId);

            if (pricelists != null)
            {
                return Ok(pricelists);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("addStation")]
        public async Task<IActionResult> AddStation(NewStationDto station)
        {
            Station newStation = new Station();
            _mapper.Map(station, newStation);

            var result = await _publicTransportRepository.AddStation(newStation);

            if (result != null)
            {
                if (station.Lines != null)
                {
                    foreach (var line in station.Lines)
                    {
                        await _publicTransportRepository.AddLineToStation(result.Id, line.Id);
                    }
                }
                
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed while creating new station");
            }
        }

        [HttpPost("addLine")]
        public async Task<IActionResult> AddLine(NewLineDto line)
        {
            var lineToAdd = new Line();

            _mapper.Map(line, lineToAdd);

            var result = await _publicTransportRepository.AddLine(lineToAdd);

            if (result != null)
            {
                if (line.Stations != null)
                {
                    foreach (var station in line.Stations)
                    {
                        await _publicTransportRepository.AddStationToLine(station.Id, result.Id);
                    }
                }
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed while creating new line");
            }
        }

        [HttpPost("addTimetable")]
        public async Task<IActionResult> AddTimetable(TimeTable timetable)
        {
            var result = await _publicTransportRepository.AddTimetable(timetable);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed while creating new timetable");
            }
        }

        [HttpPost("addPricelist")]
        public async Task<IActionResult> AddPricelist(NewPricelistDto pricelist)
        {

            var result = await _publicTransportRepository.AddPricelist(pricelist);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed while creating new pricelist");
            }
        }

        [HttpPut("updateStation")]
        public async Task<IActionResult> UpdateStation(int stationId, NewStationDto station)
        {
            var result = await _publicTransportRepository.UpdateStation(stationId, station);

            if (result != null)
            {
                if (station.Lines != null)
                {
                    foreach (var line in station.Lines)
                    {
                        await _publicTransportRepository.AddLineToStation(result.Id, line.Id);
                    }
                }
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to update station");
            }
        }

        [HttpPut("updateLine")]
        public async Task<IActionResult> UpdateLine(int lineId, NewLineDto line)
        {
            var result = await _publicTransportRepository.UpdateLine(lineId, line);

            if (result != null)
            {
                if (line.Stations != null)
                {
                    foreach (var station in line.Stations)
                    {
                        await _publicTransportRepository.AddStationToLine(station.Id, result.Id);
                    }
                }
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to update line");
            }
        }

        [HttpPut("updateTimetable")]
        public async Task<IActionResult> UpdateTimetable(int timetableId, TimeTable timetable)
        {
            var result = await _publicTransportRepository.UpdateTimetable(timetableId, timetable);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to update timetable");
            }
        }

        [HttpPut("updatePricelist")]
        public async Task<IActionResult> UpdatePricelist(int pricelistId, NewPricelistDto pricelist)
        {

            var result = await _publicTransportRepository.UpdatePricelist(pricelistId, pricelist);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to update pricelist");
            }
        }

        [HttpDelete("removeStation")]
        public async Task<IActionResult> RemoveStation(int stationId)
        {
            var result = await _publicTransportRepository.RemoveStation(stationId);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest($"Failed while deleting station with id {stationId}");
            }
        }

        [HttpDelete("removeLine")]
        public async Task<IActionResult> RemoveLine(int lineId)
        {
            var result = await _publicTransportRepository.RemoveLine(lineId);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest($"Failed while deleting line with id {lineId}");
            }
        }

        [HttpDelete("removeTimetable")]
        public async Task<IActionResult> RemoveTimetable(int timetableId)
        {
            var result = await _publicTransportRepository.RemoveTimetable(timetableId);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest($"Failed while deleting timetable with id {timetableId}");
            }
        }

        [HttpDelete("removePricelist")]
        public async Task<IActionResult> RemovePricelist(int pricelistId)
        {
            var result = await _publicTransportRepository.RemovePricelist(pricelistId);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest($"Failed while deleting pricelist with id {pricelistId}");
            }
        }

        [HttpPost("addBus")]
        public async Task<IActionResult> AddBus(Bus bus)
        {
            var result = await _publicTransportRepository.AddBus(bus);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed while creating new bus");
            }
        }

        [HttpGet("getUserDiscount")]
        public async Task<IActionResult> GetUserDiscount(string type)
        {
            var discount = await _publicTransportRepository.GetDiscount(type);

            if (discount != null)
            {
                return Ok(discount);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("updateUserDiscount")]
        public async Task<IActionResult> UpdateUserDiscount(string type, UserDiscount discount)
        {
            var result = await _publicTransportRepository.UpdateDiscount(type, discount);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to update timetable");
            }
        }
    }
}