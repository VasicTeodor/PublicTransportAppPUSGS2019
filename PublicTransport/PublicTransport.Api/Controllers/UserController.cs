using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PublicTransport.Api.Data;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPublicTransportRepository _publicTransportRepository;

        public UserController(IMapper mapper, IPublicTransportRepository publicTransportRepository)
        {
            _mapper = mapper;
            _publicTransportRepository = publicTransportRepository;
        }

        [AllowAnonymous]
        [HttpGet("pricelists")]
        public async Task<IActionResult> GetPricelists([FromQuery]bool active)
        {
            var pricelists = await _publicTransportRepository.GetPricelists(active).ToAsyncEnumerable().ToList();

            if (pricelists.Count() > 0)
            {
                return Ok(pricelists);
            }
            else
            {
                return BadRequest("Error while getting pricelists.");
            }
        }

        [HttpGet("timetables")]
        public Task<IActionResult> GetTimetables()
        {
            throw new NotImplementedException();
        }

        [HttpGet("lines")]
        public Task<IActionResult> GetLinesForMap()
        {
            throw new NotImplementedException();
        }
    }
}