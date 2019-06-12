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

        [HttpGet("getLines")]
        public Task<IActionResult> GetLines()
        {
            throw new NotImplementedException();
        }

        [HttpGet("getTimetables")]
        public Task<IActionResult> GetTimetables()
        {
            throw new NotImplementedException();
        }

        [HttpGet("getPricelist")]
        public Task<IActionResult> GetPricelist()
        {
            throw new NotImplementedException();
        }

        [HttpPost("addStation")]
        public async Task<IActionResult> AddStation(NewStationDto station)
        {
            Station newStation = new Station();
            _mapper.Map(station, newStation);

            var result = await _publicTransportRepository.AddStation(newStation);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed while creating new station");
            }
        }

        [HttpPost("addLine")]
        public Task<IActionResult> AddLine()
        {
            throw new NotImplementedException();
        }

        [HttpPost("addTimetable")]
        public Task<IActionResult> AddTimetable()
        {
            throw new NotImplementedException();
        }

        [HttpPost("addPricelist")]
        public Task<IActionResult> AddPricelist()
        {
            throw new NotImplementedException();
        }

        [HttpPut("updateStation")]
        public async Task<IActionResult> UpdateStation(int stationId, Station station)
        {
            var result = await _publicTransportRepository.UpdateStation(stationId, station);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to update station");
            }
        }

        [HttpPut("updateLine")]
        public Task<IActionResult> UpdateLine()
        {
            throw new NotImplementedException();
        }

        [HttpPut("updateTimetable")]
        public Task<IActionResult> UpdateTimetable()
        {
            throw new NotImplementedException();
        }

        [HttpPut("updatePricelist")]
        public Task<IActionResult> UpdatePricelist()
        {
            throw new NotImplementedException();
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
        public Task<IActionResult> RemoveLine()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("removeTimetable")]
        public Task<IActionResult> RemoveTimetable()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("removePricelist")]
        public Task<IActionResult> RemovePricelist()
        {
            throw new NotImplementedException();
        }
    }
}