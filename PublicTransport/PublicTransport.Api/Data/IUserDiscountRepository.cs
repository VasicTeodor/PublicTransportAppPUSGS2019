using PublicTransport.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicTransport.Api.Data
{
    public interface IUserDiscountRepository
    {
        Task<IEnumerable<UserDiscount>> GetUserDiscounts();
        Task<UserDiscount> GetUserDiscountForType(string type);
    }
}