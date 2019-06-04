using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public class UserDiscountRepository : IUserDiscountRepository
    {
        private readonly DataContext _context;

        public UserDiscountRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UserDiscount> GetUserDiscountForType(string type)
        {
            return await _context.UserDiscounts.FirstOrDefaultAsync(ud => ud.Type == type);
        }

        public async Task<IEnumerable<UserDiscount>> GetUserDiscounts()
        {
            return await _context.UserDiscounts.ToListAsync();
        }
    }
}