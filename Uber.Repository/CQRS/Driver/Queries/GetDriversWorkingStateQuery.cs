using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Uber.Boundary.CQRS.Driver.Queries
{
    public class GetDriversWorkingStateQuery : IRequest<bool>
    {
        public int DriversId { get; set; }
    }
}
