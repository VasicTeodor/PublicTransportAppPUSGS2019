using System.Threading.Tasks;
using PayPal.Api;

namespace PublicTransport.Api.Data
{
    public interface IPayPalService
    {
        Task<Payment> CreatePayment(string ticketType, int userId = -1, string email = null);
        Task<Payment> ExecutePayment(string ticketType, string paymentId, string payerID, int userId = -1, string email = null);
    }
}