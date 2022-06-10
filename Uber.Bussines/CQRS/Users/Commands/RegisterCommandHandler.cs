using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Users;
using Uber.Boundary.CQRS.Users.Commands;
using Uber.Domain.IRepository;
using Uber.Domain.Models;

namespace Uber.Bussines.CQRS.Users.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserResponse>
    {
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            User user = new User()
            {
                Id = command.,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Password = command.Password,
                Role = command.Role,
                VehicleBrand = command.VehicleBrand,
                LicensePlate = command.LicensePlate,
                PricePerKm = command.PricePerKm,
                
            };

            return await _userRepository.Register(user);
        }
    }

}
