using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Driver.Commands;
using Uber.Boundary.Helpers;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Driver.Commands
{
    public class AcceptOrDeclineReservationCommandHandler : IRequestHandler<AcceptOrDeclineReservationCommand>
    {
        private readonly IDriversRepository _driverRepo;
        public AcceptOrDeclineReservationCommandHandler(IDriversRepository driverRepo)
        {
            _driverRepo = driverRepo;
        }

        public async Task<Unit> Handle(AcceptOrDeclineReservationCommand cmd, CancellationToken cancellationToken)
        {
            await _driverRepo.AcceptOrDeclineReservation(cmd.ReservationId, (Domain.Helpers.Enums.ReservationDecisionEnum)cmd.Decision);
            return Unit.Value;
        }
    }
}
