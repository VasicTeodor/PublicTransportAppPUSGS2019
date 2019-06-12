﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicTransport.Api.Data;
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

        [HttpGet("getPricelist")]
        public async Task<IActionResult> GetPricelists()
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

        [HttpPost("addStation")]
        public async Task<IActionResult> AddStation(Station station)
        {
            var result = await _publicTransportRepository.AddStation(station);

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
        public async Task<IActionResult> AddLine(Line line)
        {
            var result = await _publicTransportRepository.AddLine(line);

            if (result != null)
            {
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
        public async Task<IActionResult> AddPricelist(PricelistItem pricelist)
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
        public async Task<IActionResult> UpdateLine(int lineId, Line line)
        {
            var result = await _publicTransportRepository.UpdateLine(lineId, line);

            if (result != null)
            {
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
        public async Task<IActionResult> UpdatePricelist(int pricelistId, PricelistItem pricelist)
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
    }
}