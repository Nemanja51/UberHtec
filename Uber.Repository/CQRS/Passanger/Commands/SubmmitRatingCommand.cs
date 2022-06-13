using MediatR;
using Uber.Boundary.Helpers;

namespace Uber.Boundary.CQRS.Passanger.Commands
{
    public class SubmmitRatingCommand : IRequest<string>
    {
        public RateDriver RateDriver { get; set; }
    }
}
