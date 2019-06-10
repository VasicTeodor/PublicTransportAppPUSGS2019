using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicTransport.Api.Data;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Controller")]
    [ApiController]
    public class ModeratorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPublicTransportRepository _publicTransportRepository;
        private readonly UserManager<User> _userManager;

        public ModeratorController(IMapper mapper, IPublicTransportRepository publicTransportRepository,
            UserManager<User> userManager)
        {
            _mapper = mapper;
            _publicTransportRepository = publicTransportRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _publicTransportRepository.GetTickets();

            if (tickets != null)
            {
                return Ok(tickets);
            }
            else
            {
                return BadRequest("Error while getting tickets!");
            }
        }

        [HttpPut]
        public async Task<IActionResult> ValidateUserAccount([FromForm]int userId, [FromForm]bool valid)
        {
            var result = await _publicTransportRepository.ValidateUserAccount(userId, valid);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("validateTicket")]
        public async Task<IActionResult> ValidateUserTicket([FromForm]int ticketId)
        {
            var result = await _publicTransportRepository.ValidateUserTicket(ticketId);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}