using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Passanger.Commands;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Passanger.Commands
{
    public class SendReservationRequestCommandHandler : IRequestHandler<SendReservationRequestCommand>
    {
        private readonly IPassangerRepository _passangerRepo;
        public SendReservationRequestCommandHandler(IPassangerRepository passangerRepo)
        {
            _passangerRepo = passangerRepo;
        }

        public async Task<Unit> Handle(SendReservationRequestCommand cmd, CancellationToken cancellationToken)
        {
            await _passangerRepo.SendReservationRequest(cmd.Reservation);
            return Unit.Value;
        }
    }
}
