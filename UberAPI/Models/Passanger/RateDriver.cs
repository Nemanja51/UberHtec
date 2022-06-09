using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UberAPI.Models.Passanger
{
    public class RateDriver
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public int PassangerId { get; set; }
        [Range(1,5)]
        public int Rate { get; set; }
        public string Comment { get; set; }
        public DateTime DateTimeOfRate { get; set; }

        //fk
        [ForeignKey("DriverId")]
        public virtual User Driver { get; set; }
        [ForeignKey("PassangerId")]
        public virtual User Passanger { get; set; }
    }
}
