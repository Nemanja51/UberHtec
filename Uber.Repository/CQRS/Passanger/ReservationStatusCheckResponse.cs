using System;
using Uber.Boundary.Helpers;

namespace Uber.Boundary.CQRS.Passanger
{
    public class ReservationStatusCheckResponse
    {
        public ReservationStatusEnum ReservationStatus { get; set; }
        public TimeSpan ReservationTimePassed { get; set; }
    }
}
