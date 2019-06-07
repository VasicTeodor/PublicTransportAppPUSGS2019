using System.Collections.Generic;
using System.Threading.Tasks;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public interface IPricelistItemRepository
    {
        Task<IEnumerable<PricelistItem>> GetPricelistItemsByActive(bool active = false);
        Task<IEnumerable<PricelistItem>> GetPricelistItems();
        Task<PricelistItem> GetPriceListItem(int id);
        Task<PricelistItem> GetPriceListItemForTicketType(string type);
    }
}