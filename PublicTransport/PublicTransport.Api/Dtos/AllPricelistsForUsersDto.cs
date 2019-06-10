using System.Collections.Generic;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Dtos
{
    public class AllPricelistsForUsersDto
    {
        public List<PricelistItem> RegularUserPricelist { get; set; }
        public List<PricelistItem> StudentPricelist { get; set; }
        public List<PricelistItem> SeniorPricelist { get; set; }
    }
}