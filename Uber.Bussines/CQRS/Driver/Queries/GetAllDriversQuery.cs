using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Models;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Driver.Queries
{
    public class GetAllDriversQuery : IRequest<List<User>>
    {
        public class GetAllDriversQueryHandler : IRequestHandler<GetAllDriversQuery, List<User>>
        {
            private readonly IDriversRepository _driverRepo;
            public GetAllDriversQueryHandler(IDriversRepository driverRepo)
            {
                _driverRepo = driverRepo;
            }

            public async Task<List<User>> Handle(GetAllDriversQuery query, CancellationToken cancellationToken)
            {
                return await _driverRepo.GetAllDrivers();
            }
        }
    }
}
