using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Uber.Boundary.CQRS.Passanger.Commands
{
    public class SendReservationRequestCommand : IRequest
    {
        public Reservation Reservation { get; set; }
    }
}
