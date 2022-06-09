using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Models;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Users.Commands
{
    public class RegisterCommand : IRequest<User>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string VehicleBrand { get; set; }
        public string LicensePlate { get; set; }
        public decimal PricePerKm { get; set; }

        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, User>
        {
            private readonly IUserRepository _userRepository;

            public RegisterCommandHandler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<User> Handle(RegisterCommand command, CancellationToken cancellationToken)
            {
                User user = new User()
                {
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    Email = command.Email,
                    Password = command.Password,
                    Role = command.Role,
                    VehicleBrand = command.VehicleBrand,
                    LicensePlate = command.LicensePlate,
                    PricePerKm = command.PricePerKm
                };

                return await _userRepository.Register(user);
            }
        }
    }
}
