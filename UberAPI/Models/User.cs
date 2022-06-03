using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UberAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Role { get; set; }
        [NotMapped]
        public string  Token { get; set; }

        //driver
        public string VehicleBrand { get; set; }
        public string LicensePlate { get; set; }
        public decimal PricePerKm { get; set; }
    }
}
