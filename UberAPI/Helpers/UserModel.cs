namespace UberAPI.Helpers
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

        //driver
        public string VehicleBrand { get; set; }
        public string LicensePlate { get; set; }
        public decimal PricePerKm { get; set; }
    }
}
