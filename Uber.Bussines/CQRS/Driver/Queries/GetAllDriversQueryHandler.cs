using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Driver.Queries;
using Uber.Boundary.CQRS.Users;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Driver.Queries
{
    public class GetAllDriversQueryHandler : IRequestHandler<GetAllDriversQuery, List<UserResponse>>
    {
        private readonly IDriversRepository _driverRepo;
        public GetAllDriversQueryHandler(IDriversRepository driverRepo)
        {
            _driverRepo = driverRepo;
        }

        public async Task<List<UserResponse>> Handle(GetAllDriversQuery query, CancellationToken cancellationToken)
        {
            var result = await _driverRepo.GetAllDrivers();

            List<UserResponse> userList = new List<UserResponse>();

            foreach (var user in result)
            {
                UserResponse ur = new UserResponse() 
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

                userList.Add(ur);
            }

            return userList;
        }
    }
}
