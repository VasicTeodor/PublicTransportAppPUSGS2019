using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicTransport.Api.Data;

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
        public Task<IActionResult> GetStations()
        {
            throw new NotImplementedException();
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
        public Task<IActionResult> AddStation()
        {
            throw new NotImplementedException();
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

        [HttpPut("addStation")]
        public Task<IActionResult> UpdateStation()
        {
            throw new NotImplementedException();
        }

        [HttpPut("addLine")]
        public Task<IActionResult> UpdateLine()
        {
            throw new NotImplementedException();
        }

        [HttpPut("addTimetable")]
        public Task<IActionResult> UpdateTimetable()
        {
            throw new NotImplementedException();
        }

        [HttpPut("addPricelist")]
        public Task<IActionResult> UpdatePricelist()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("removeStation")]
        public Task<IActionResult> RemoveStation()
        {
            throw new NotImplementedException();
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