using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public class PricelistItemRepository : IPricelistItemRepository
    {
        private readonly DataContext _context;

        public PricelistItemRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<PricelistItem> GetPriceListItem(int id)
        {
            return await _context.PricelistItems.Include(p => p.Item).Include(p => p.Pricelist)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PricelistItem>> GetPricelistItemsByActive(bool active = false)
        {
            return await _context.PricelistItems.Include(p => p.Item).Include(p => p.Pricelist)
                .Where(p => p.Pricelist.Active == active).ToListAsync();
        }

        public async Task<IEnumerable<PricelistItem>> GetPricelistItems()
        {
            return await _context.PricelistItems.Include(p => p.Pricelist).Include(p => p.Item).ToListAsync();
        }

        public async Task<PricelistItem> GetPriceListItemForTicketType(string type)
        {
            return await _context.PricelistItems.Include(p => p.Item)
                .Include(p => p.Pricelist)
                .FirstOrDefaultAsync(p => p.Item.Type == type && p.Pricelist.Active == true);
        }
    }
}