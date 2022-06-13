using MediatR;

namespace Uber.Boundary.CQRS.Driver.Commands
{
    public class DeclineReservationAfter2MinsOfPendingCommand : IRequest
    {
        public int ReservationId { get; set; }
    }
}
