using System;
using System.Collections.Generic;
using System.Text;

namespace Uber.Boundary.CQRS.Passanger
{
    public class DriversLocationResponse
    {
        public int Id { get; set; }
        public int DriversId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
