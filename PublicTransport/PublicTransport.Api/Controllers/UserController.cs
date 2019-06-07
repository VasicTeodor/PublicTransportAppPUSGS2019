using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PublicTransport.Api.Data;
using PublicTransport.Api.Dtos;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPublicTransportRepository _publicTransportRepository;
        private readonly UserManager<User> _userManager;

        public UserController(IMapper mapper, IPublicTransportRepository publicTransportRepository, UserManager<User> userManager)
        {
            _mapper = mapper;
            _publicTransportRepository = publicTransportRepository;
            _userManager = userManager;
        }

        [Authorize(Roles = "Controller, Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _publicTransportRepository.GetUsers();

            if (users != null)
            {
                return Ok(users);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Passenger, Controller, Admin")]
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _publicTransportRepository.GetUser(id);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Passenger")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id,UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userFromRepo = await _publicTransportRepository.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            var result = await _userManager.UpdateAsync(userFromRepo);
            
            if (result.Succeeded)
            {
                result = await _userManager.ChangePasswordAsync(userFromRepo, userForUpdateDto.OldPassword, userForUpdateDto.Password);

                if (result.Succeeded)
                {
                    return NoContent();
                }
                else
                {
                    throw new Exception($"Updating user {id} failed on save!");
                }
            }
            else
            {
                throw new Exception($"Updating user {id} failed on save!");
            }
        }

        [Authorize(Roles = "Passenger")]
        [HttpPut("buyTicket")]
        public async Task<IActionResult> BuyTicket(string type,int userId = -1, string email = null)
        {
            var result = await _publicTransportRepository.BuyTicketAsync(type, userId, email);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [Authorize(Roles = "Passenger")]
        [HttpPost("addDocument")]
        public Task<IActionResult> AddDocumentForUse(int id)
        {
            throw new NotImplementedException();
        }
    }
}