using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.Helpers;

namespace Uber.Boundary.CQRS.Driver.Commands
{
    public class AcceptOrDeclineReservationCommand : IRequest
    {
        public int ReservationId { get; set; }
        public ReservationDecisionEnum Decision { get; set; }
       
    }
}
