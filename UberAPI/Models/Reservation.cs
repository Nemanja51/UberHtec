using System.ComponentModel.DataAnnotations.Schema;
using UberAPI.Helpers.Enums;

namespace UberAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public int PassangerId { get; set; }
        public ReservationStatusEnum ReservationStatus { get; set; }

        //fk
        [ForeignKey("DriverId")]
        public virtual User Driver { get; set; }
        [ForeignKey("PassangerId")]
        public virtual User Passanger { get; set; }
    }
}
