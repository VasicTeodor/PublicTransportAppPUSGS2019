using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using PayPal.Api;
using PublicTransport.Api.Models;
using Item = PayPal.Api.Item;

namespace PublicTransport.Api.Data
{
    public class PayPalService : IPayPalService
    {
        private readonly IPublicTransportRepository _publicTransportRepository;
        private readonly IMapper _mapper;
        private string _accessToken;

        public string Token
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }


        public PayPalService(IPublicTransportRepository publicTransportRepository, IMapper mapper)
        {
            _publicTransportRepository = publicTransportRepository;
            _mapper = mapper;
        }
        public async Task<Payment> CreatePayment(string ticketType, int userId = -1, string email = null)
        {
            decimal price = 0;
            var pricelist = await _publicTransportRepository.GetTicketPrice(ticketType);

            if (userId != -1)
            {
                var user = await _publicTransportRepository.GetUser(userId);

                if (user.Verified == false)
                {
                    return null;
                }
            }

            price = await CalculatePrice(pricelist, userId);

            Payment createdPayment = new Payment();
            var config = ConfigManager.Instance.GetProperties();
            _accessToken = new OAuthTokenCredential(config).GetAccessToken();

            var apiContext = new APIContext(_accessToken)
            {
                Config = ConfigManager.Instance.GetProperties()
            };

            try
            {
                Payment payment = new Payment()
                {
                    intent = "sale",
                    payer = new Payer() { payment_method = "paypal" },
                    transactions = new List<Transaction>()
                    {
                        new Transaction()
                        {
                            item_list = new ItemList()
                            {
                                items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        description = "Ticket for public transport",
                                        name = $"{pricelist.Item.Type} ticket",
                                        currency = "EUR",
                                        price = price.ToString("0.##", CultureInfo.InvariantCulture),
                                        sku = "sku",
                                        quantity = "1"
                                    }
                                }
                            },
                            amount = new Amount()
                            {
                                currency = "EUR",
                                total = price.ToString("0.##" ,CultureInfo.InvariantCulture)
                            },
                            description = pricelist.Item.Type
                        }
                    },
                    redirect_urls = new RedirectUrls()
                    {
                        cancel_url = "http://localhost:5000/api/paypal/cancel",
                        return_url = $"http://localhost:5000/api/paypal/success?ticketType={ticketType}&userId={userId}&email={email}"
                    }
                };

                createdPayment = await Task.Run(() => payment.Create(apiContext));

            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return null;
            }

            return createdPayment;
        }

        public async Task<Payment> ExecutePayment(string ticketType, string paymentId, string payerID, int userId = -1, string email = null)
        {
            var config = ConfigManager.Instance.GetProperties();
            _accessToken = new OAuthTokenCredential(config).GetAccessToken();

            var apiContext = new APIContext(_accessToken)
            {
                Config = ConfigManager.Instance.GetProperties()
            };

            PaymentExecution paymentExecution = new PaymentExecution() {payer_id = payerID };

            Payment payment = new Payment() {id = paymentId};

            Payment executedPayment = await Task.Run(() => payment.Execute(apiContext, paymentExecution));

            var result = await _publicTransportRepository.BuyTicketAsync(ticketType, userId, email);

            var payPalInfo = _mapper.Map<PayPalInfo>(executedPayment);

            payPalInfo.UserId = userId;

            var payPalSaveResult = await _publicTransportRepository.SavePayPalPayementInfo(payPalInfo);

            return executedPayment;
        }

        private async Task<decimal> CalculatePrice(PricelistItem pricelist, int userId)
        {
            User user = null;
            decimal price = 0;

            if (pricelist != null)
            {
                price = pricelist.Price;
            }

            if (userId != -1)
            {
                user = await _publicTransportRepository.GetUser(userId);
            }

            if (user != null)
            {
                var discount = await _publicTransportRepository.GetDiscount(user.UserType);

                price = price - (price * ((decimal) discount.Value / 100));
            }

            price = price / 118;

            return price;
        }
    }
}