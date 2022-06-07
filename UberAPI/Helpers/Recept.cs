using System;

namespace UberAPI.Helpers
{
    public class Recept
    {
        public string DriversFullName { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public TimeSpan RideDuration { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
    }
}
