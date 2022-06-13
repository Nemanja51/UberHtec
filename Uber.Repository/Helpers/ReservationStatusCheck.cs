using System;

namespace Uber.Boundary.Helpers
{
    public class ReservationStatusCheck
    {
        public ReservationStatusEnum ReservationStatus { get; set; }
        public TimeSpan ReservationTimePassed { get; set; }
    }
}
