using MediatR;

namespace Uber.Boundary.CQRS.Passanger.Queries
{
    public class IsThereReservationRequestPendingQuery : IRequest<bool>
    {
        public int PassangerId { get; set; }
    }
}
