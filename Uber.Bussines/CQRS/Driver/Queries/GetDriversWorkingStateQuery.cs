using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Driver.Queries
{
    public class GetDriversWorkingStateQuery : IRequest<bool>
    {
        public int DriversId { get; set; }
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
}
