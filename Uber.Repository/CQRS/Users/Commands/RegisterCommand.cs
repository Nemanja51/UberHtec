using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Domain.Models;

namespace Uber.Boundary.CQRS.Users.Commands
{
    public class RegisterCommand : IRequest<UserResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string VehicleBrand { get; set; }
        public string LicensePlate { get; set; }
        public decimal PricePerKm { get; set; }
    }
}
