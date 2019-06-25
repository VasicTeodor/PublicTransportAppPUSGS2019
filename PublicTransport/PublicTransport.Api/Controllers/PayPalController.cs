using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;
using PublicTransport.Api.Data;

namespace PublicTransport.Api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        private readonly IPayPalService _payPalService;

        public PayPalController(IPayPalService payPalService)
        {
            _payPalService = payPalService;
        }
        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> CreatePayment(string ticketType, int userId = -1, string email = null)
        {
            var result = await _payPalService.CreatePayment(ticketType, userId, email);

            if (result != null)
            {
                foreach (var link in result.links)
                {
                    if (link.rel.Equals("approval_url"))
                    {
                        return Ok(new { address = link.href });
                    }
                }
            }

            return BadRequest("Your account is not verificated!");
        }

        [HttpGet]
        [Route("success")]
        public async Task<IActionResult> ExecutePayment(string ticketType, string paymentId, string token, string payerID, int userId = -1, string email = null)
        {
            Payment result = await _payPalService.ExecutePayment(ticketType, paymentId, payerID, userId, email);

            return Redirect("http://localhost:4200/tickets;success=1");
        }

        [HttpGet]
        [Route("cancel")]
        public async Task<IActionResult> CancelPayment()
        {
            return Redirect("http://localhost:4200/tickets;success=0");
        }
    }
}