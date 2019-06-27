using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PublicTransport.Api.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PublicTransport.Api.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems,
            int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        public static async Task<User> GetUserById(this UserManager<User> userManager, int id)
        {
            return await userManager.Users.Include(u => u.Address)
                .Include(u => u.Tickets)
                .ThenInclude(t => t.PriceInfo)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public static string GetTimetableForToday(this DateTime theDateTime)
        {
            var today = DateTime.Now;
            switch (today.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return "Saturday";
                case DayOfWeek.Sunday:
                    return "Sunday";
                default:
                    return "Working Days";
            }
        }
    }
}