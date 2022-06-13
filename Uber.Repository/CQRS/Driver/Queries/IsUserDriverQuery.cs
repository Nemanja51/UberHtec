using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Uber.Boundary.CQRS.Driver.Queries
{
    public class IsUserDriverQuery : IRequest<bool>
    {
        public string DriversId { get; set; }
    }
}
