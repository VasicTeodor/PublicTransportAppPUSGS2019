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
        [HttpPut]
        public Task<IActionResult> ValidateUserAccount(int userId, bool valid)
        {
            throw new NotImplementedException();
        }

        [HttpPut("validateTicket")]
        public Task<IActionResult> ValidateUserTicket(int ticketId, bool valid)
        {
            throw new NotImplementedException();
        }
    }
}