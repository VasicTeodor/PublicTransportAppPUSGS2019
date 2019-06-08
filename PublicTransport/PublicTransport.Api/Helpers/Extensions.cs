using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PublicTransport.Api.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PublicTransport.Api.Helpers
{
    public static class Extensions
    {
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