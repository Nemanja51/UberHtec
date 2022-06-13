using MediatR;
using System.Collections.Generic;
using Uber.Boundary.CQRS.Passanger;

namespace Uber.Boundary.CQRS.Driver.Queries
{
    public class GetAllPendingReservationsQuery : IRequest<List<ReservationResponse>>
    {
        public int DriversId { get; set; }
    }
}
