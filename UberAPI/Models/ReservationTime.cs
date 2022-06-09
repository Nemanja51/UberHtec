using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UberAPI.Models
{
    public class ReservationTime
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        //fk
        [ForeignKey("ReservationId")]
        public virtual Reservation Reservation { get; set; }
    }
}
