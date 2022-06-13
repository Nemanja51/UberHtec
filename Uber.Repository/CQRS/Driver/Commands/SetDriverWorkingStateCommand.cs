using MediatR;

namespace Uber.Boundary.CQRS.Driver.Commands
{
    public class SetDriverWorkingStateCommand : IRequest<bool>
    {
        public string DriversId { get; set; }
    }
}
