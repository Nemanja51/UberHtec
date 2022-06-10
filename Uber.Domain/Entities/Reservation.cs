using System;
using System.ComponentModel.DataAnnotations.Schema;
using Uber.Domain.Helpers.Enums;

namespace Uber.Domain.Models
{
    public class Reservation
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

        //fk
        [ForeignKey("DriverId")]
        public virtual User Driver { get; set; }
        [ForeignKey("PassangerId")]
        public virtual User Passanger { get; set; }
    }
}
