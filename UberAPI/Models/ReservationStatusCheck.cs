using System;
using UberAPI.Helpers.Enums;

namespace UberAPI.Models
{
    public class ReservationStatusCheck
    {
        public ReservationStatusEnum ReservationStatus { get; set; }
        public TimeSpan ReservationTimePassed { get; set; }
    }
}
