using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Driver.Commands;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Driver.Commands
{
    public class DeclineReservationAfter2MinsOfPendingCommandHandler : IRequestHandler<DeclineReservationAfter2MinsOfPendingCommand>
    {
        private readonly IDriversRepository _driverRepo;
        public DeclineReservationAfter2MinsOfPendingCommandHandler(IDriversRepository driverRepo)
        {
            _driverRepo = driverRepo;
        }

        public async Task<Unit> Handle(DeclineReservationAfter2MinsOfPendingCommand cmd, CancellationToken cancellationToken)
        {
            await _driverRepo.DeclineReservationAfter2MinsOfPending(cmd.ReservationId);
            return Unit.Value;
        }
    }
}
