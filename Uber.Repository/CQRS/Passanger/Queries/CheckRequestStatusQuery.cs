using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Uber.Boundary.CQRS.Passanger.Queries
{
    public class CheckRequestStatusQuery : IRequest<ReservationStatusCheckResponse>
    {
        public int PassangerId { get; set; }
    }
}
