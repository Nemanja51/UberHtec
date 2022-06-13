using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Passanger;
using Uber.Boundary.Helpers;

namespace Uber.Boundary.CQRS.Passanger.Queries
{
    public class IdleVehiclesInProximityQuery : IRequest<List<DriversLocationResponse>>
    {
        public Cordinates Cordinates { get; set; }
    }
}
