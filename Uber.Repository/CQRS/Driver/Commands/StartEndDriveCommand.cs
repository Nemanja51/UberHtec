using MediatR;
using Uber.Boundary.Helpers;

namespace Uber.Boundary.CQRS.Driver.Commands
{
    public class StartEndDriveCommand : IRequest<StartEndEnum>
    {
        public int ReservationId { get; set; }
    }
}
