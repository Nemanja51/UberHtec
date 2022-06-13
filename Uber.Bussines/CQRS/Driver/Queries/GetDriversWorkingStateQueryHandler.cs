using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Driver.Queries;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Driver.Queries
{
    public class GetDriversWorkingStateQueryHandler : IRequestHandler<GetDriversWorkingStateQuery, bool>
    {
        private readonly IDriversRepository _driverRepo;
        public GetDriversWorkingStateQueryHandler(IDriversRepository driverRepo)
        {
            _driverRepo = driverRepo;
        }

        public async Task<bool> Handle(GetDriversWorkingStateQuery query, CancellationToken cancellationToken)
        {
            return await _driverRepo.GetDriversWorkingState(query.DriversId);
        }
    }
}
