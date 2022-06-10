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
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Password = command.Password,
                Role = command.Role,
                VehicleBrand = command.VehicleBrand,
                LicensePlate = command.LicensePlate,
                PricePerKm = command.PricePerKm,
                
            };

            var userObj = await _userRepository.Register(user);

            return new UserResponse()
            {
                Id = userObj.Id,
                FirstName = userObj.FirstName,
                LastName = userObj.LastName,
                Password = userObj.Password,
                Email = userObj.Email,
                Role = userObj.Role,
                Token = userObj.Token,
                VehicleBrand = userObj.VehicleBrand,
                LicensePlate = userObj.LicensePlate,
                PricePerKm = userObj.PricePerKm
            };
        }
    }

}
