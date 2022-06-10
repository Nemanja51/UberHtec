using System;
using Uber.Domain.Helpers.Enums;

namespace Uber.Domain.Models
{
    public class ReservationStatusCheck
    {
        public ReservationStatusEnum ReservationStatus { get; set; }
        public TimeSpan ReservationTimePassed { get; set; }
    }
}
