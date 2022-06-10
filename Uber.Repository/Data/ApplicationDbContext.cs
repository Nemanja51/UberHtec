using Microsoft.EntityFrameworkCore;
using Uber.Domain.Models;

namespace Uber.Boundary.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<DriversLocation> DriversLocations { get; set; }
        public DbSet<DriversAvailability> DriversAvailability { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationTime> ReservationTimes { get; set; }
        public DbSet<RateDriver> DriverRates { get; set; }
    }
}
