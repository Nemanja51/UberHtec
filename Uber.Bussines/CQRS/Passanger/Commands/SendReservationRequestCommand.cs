using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Models;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Passanger.Commands
{
    public class SendReservationRequestCommand : IRequest
    {
        public Reservation Reservation { get; set; }

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
}
