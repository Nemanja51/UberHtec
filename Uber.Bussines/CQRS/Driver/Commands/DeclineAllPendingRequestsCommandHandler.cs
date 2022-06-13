using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Driver.Commands;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Driver.Commands
{
    public class DeclineAllPendingRequestsCommandHandler : IRequestHandler<DeclineAllPendingRequestsCommand>
    {
        private readonly IDriversRepository _driverRepo;
        public DeclineAllPendingRequestsCommandHandler(IDriversRepository driverRepo)
        {
            _driverRepo = driverRepo;
        }

        public async Task<Unit> Handle(DeclineAllPendingRequestsCommand cmd, CancellationToken cancellationToken)
        {
            await _driverRepo.DeclineAllPendingRequests(cmd.DriversId, cmd.ReservationId);
            return Unit.Value;
        }
    }

}
