using MediatR;

namespace Uber.Boundary.CQRS.Driver.Commands
{
    public class DeclineAllPendingRequestsCommand : IRequest
    {
        public int ReservationId { get; set; }
        public int DriversId { get; set; }
    }
}
