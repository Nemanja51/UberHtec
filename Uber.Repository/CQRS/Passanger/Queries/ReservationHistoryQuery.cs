using MediatR;
using System.Collections.Generic;

namespace Uber.Boundary.CQRS.Passanger.Queries
{
    public class ReservationHistoryQuery : IRequest<List<ReservationResponse>>
    {
        public int PassangerId { get; set; }
    }
}
