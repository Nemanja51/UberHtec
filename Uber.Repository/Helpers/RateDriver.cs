using System;

namespace Uber.Boundary.Helpers
{
    public class RateDriver
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public int PassangerId { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public DateTime DateTimeOfRate { get; set; }
    }
}
