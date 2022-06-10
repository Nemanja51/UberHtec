using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Users;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Users.Queries
{
    public class AuthenticateQueryHandler : IRequestHandler<AuthenticateQuery, UserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthenticateQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserResponse> Handle(AuthenticateQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Authenticate(query.FirstName, query.LastName, query.Password);
            
            return new UserResponse() 
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Email = user.Email,
                Role = user.Role,
                Token = user.Token,
                VehicleBrand = user.VehicleBrand,
                LicensePlate = user.LicensePlate,
                PricePerKm = user.PricePerKm
            };
        }
    }

}
