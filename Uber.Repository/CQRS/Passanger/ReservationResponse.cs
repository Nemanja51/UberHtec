using System;
using System.Collections.Generic;
using System.Text;
using Uber.Boundary.Helpers;

namespace Uber.Boundary.CQRS.Passanger
{
    public class ReservationResponse
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public int PassangerId { get; set; }
        public ReservationStatusEnum ReservationStatus { get; set; }
        public DateTime ReservationTime { get; set; }
        public DateTime StatusChangeTime { get; set; }

        //passanger Cordinates
        public double PassangersCurrentLocationLatitude { get; set; }
        public double PassangersCurrentLocationLongitude { get; set; }
        public double PassangersDesiredLocationLatitude { get; set; }
        public double PassangersDesiredLocationLongitude { get; set; }
    }
}
