using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public class DataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>,
        UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
        public DbSet<Address> Adresses { get; set; }
        public DbSet<Bus> Busses { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Pricelist> Pricelists { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<StationLine> StationLines { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<PricelistItem> PricelistItems { get; set; }
        public DbSet<UserDiscount> UserDiscounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StationLine>()
                .HasKey(k => new { k.LineId, k.StationId });

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }
    }
}