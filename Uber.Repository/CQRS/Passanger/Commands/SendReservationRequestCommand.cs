using MediatR;
using Uber.Boundary.Helpers;

namespace Uber.Boundary.CQRS.Passanger.Commands
{
    public class SendReservationRequestCommand : IRequest
    {
        public Reservation Reservation { get; set; }
    }
}
