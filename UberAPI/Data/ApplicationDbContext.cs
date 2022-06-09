using Microsoft.EntityFrameworkCore;
using UberAPI.Models;
using UberAPI.Models.Driver;
using UberAPI.Models.Passanger;

namespace UberAPI.Data
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
