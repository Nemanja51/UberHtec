using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Helpers.Enums;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Driver.Commands
{
    public class AcceptOrDeclineReservationCommand : IRequest
    {
        public int ReservationId { get; set; }
        public ReservationDecisionEnum Decision { get; set; }
        public class AcceptOrDeclineReservationCommandHandler : IRequestHandler<AcceptOrDeclineReservationCommand>
        {
            private readonly IDriversRepository _driverRepo;
            public AcceptOrDeclineReservationCommandHandler(IDriversRepository driverRepo)
            {
                _driverRepo = driverRepo; 
            }

            public async Task<Unit> Handle(AcceptOrDeclineReservationCommand cmd, CancellationToken cancellationToken)
            {
                await _driverRepo.AcceptOrDeclineReservation(cmd.ReservationId, cmd.Decision);
                return Unit.Value;
            }
        }
    }
}
