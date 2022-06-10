using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Driver.Commands
{
    public class DeclineAllPendingRequestsCommand : IRequest
    {
        public int ReservationId { get; set; }
        public int DriversId { get; set; }
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
}
