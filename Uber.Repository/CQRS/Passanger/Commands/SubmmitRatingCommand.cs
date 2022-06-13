using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Uber.Boundary.CQRS.Passanger.Commands
{
    public class SubmmitRatingCommand : IRequest<string>
    {
        public RateDriver RateDriver { get; set; }
    }
}
