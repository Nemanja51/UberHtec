using MediatR;
using Uber.Boundary.Helpers;

namespace Uber.Boundary.CQRS.Driver.Commands
{
    public class SetLocationCommand : IRequest
    {
        public int DriversId { get; set; }
        public Cordinates NewCordinates { get; set; }
    }
}
