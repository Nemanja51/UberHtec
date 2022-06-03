using System.ComponentModel.DataAnnotations.Schema;

namespace UberAPI.Models.Driver
{
    public class DriversLocation
    {
        public int Id { get; set; }
        public int DriversId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        //fk
        [ForeignKey("DriversId")]
        public virtual User User { get; set; }
    }
}
