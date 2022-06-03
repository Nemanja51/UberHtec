using System.ComponentModel.DataAnnotations.Schema;

namespace UberAPI.Models.Driver
{
    public class DriversAvailability
    {
        public int Id { get; set; }
        public int DriversId { get; set; }
        public bool Available { get; set; }

        //fk
        [ForeignKey("DriversId")]
        public virtual User Users { get; set; }
    }
}
