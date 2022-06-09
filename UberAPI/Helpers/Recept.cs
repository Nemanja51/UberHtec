using System;

namespace UberAPI.Helpers
{
    public class Recept
    {
        public string DriversFullName { get; set; }
        public string VehicleBrand { get; set; }
        public string LicancePlate { get; set; }
        public double Milage { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public TimeSpan RideDuration { get; set; }
        public string Destination { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
