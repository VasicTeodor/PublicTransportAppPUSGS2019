using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using PublicTransport.Api.Data;
using PublicTransport.Api.Dtos;
using PublicTransport.Api.Helpers;
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
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public UserController(IMapper mapper, IPublicTransportRepository publicTransportRepository,
            UserManager<User> userManager, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _mapper = mapper;
            _publicTransportRepository = publicTransportRepository;
            _userManager = userManager;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }

        [Authorize(Roles = "Controller, Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers(string accountStatus = null)
        {
            var users = await _publicTransportRepository.GetUsers(accountStatus);

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

        [AllowAnonymous]
        [HttpPut("buyTicketUnRegistered")]
        public async Task<IActionResult> BuyTicketUnregistered([FromForm]string type, [FromForm]int userId = -1, [FromForm]string email = null)
        {
            userId = -1;
            type = "Hourly";
            var result = await _publicTransportRepository.BuyTicketAsync(type, userId, email);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [Authorize(Roles = "Passenger")]
        [HttpPut("buyTicket")]
        public async Task<IActionResult> BuyTicket([FromForm]string type, [FromForm]int userId = -1, [FromForm]string email = null)
        {
            var result = await _publicTransportRepository.BuyTicketAsync(type, userId, email);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [Authorize(Roles = "Passenger")]
        [HttpPost("addDocument/{userId}")]
        public async Task<IActionResult> AddDocumentForUse(int userId, [FromForm]PhotoUploadDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userFromRepo = await _publicTransportRepository.GetUser(userId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(600).Height(500).Crop("fill")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            _mapper.Map(photoForCreationDto, userFromRepo);

            var result = await _userManager.UpdateAsync(userFromRepo);

            if (result.Succeeded)
            {
                return Ok(photoForCreationDto);
            }

            return BadRequest("Could not add the photo!");
        }
    }
}