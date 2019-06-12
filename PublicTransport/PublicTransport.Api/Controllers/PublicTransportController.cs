using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicTransport.Api.Data;
using PublicTransport.Api.Helpers;

namespace PublicTransport.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicTransportController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPublicTransportRepository _publicTransportRepository;

        public PublicTransportController(IMapper mapper, IPublicTransportRepository publicTransportRepository)
        {
            _mapper = mapper;
            _publicTransportRepository = publicTransportRepository;
        }

        [AllowAnonymous]
        [HttpGet("pricelists")]
        public async Task<IActionResult> GetPricelists([FromQuery]bool active, [FromQuery]int userId = -1, [FromQuery]bool pricelistForAll = false)
        {
            var pricelists = await _publicTransportRepository.GetPricelists(active, userId);

            if (pricelistForAll)
            {
                var result = await _publicTransportRepository.CalculateAllPricelists(pricelists.ToList());

               return Ok(result);
            }
            else
            {
                if (pricelists.Count() > 0)
                {
                    return Ok(pricelists);
                }
                else
                {
                    return BadRequest("Error while getting pricelists.");
                }
            }
        }

        [AllowAnonymous]
        [HttpGet("timetables")]
        public async Task<IActionResult> GetTimetables([FromQuery]string type = "In City", [FromQuery]string day = null)
        {
            if (day == null)
            {
                day = DateTime.Now.GetTimetableForToday();
            }

            var timetables = await _publicTransportRepository.GetTimetables(type, day);

            if (timetables.Count() > 0)
            {
                return Ok(timetables);
            }
            else
            {
                return BadRequest("Error while getting timetables.");
            }
        }

        [HttpGet("lines")]
        public Task<IActionResult> GetLinesForMap()
        {
            throw new NotImplementedException();
        }
    }
}