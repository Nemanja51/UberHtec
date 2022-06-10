
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Driver.Commands
{
    public class SetDriverWorkingStateCommand : IRequest<bool>
    {
        public string DriversId { get; set; }
        public class SetDriverWorkingStateCommandHandler : IRequestHandler<SetDriverWorkingStateCommand, bool>
        {
            private readonly IDriversRepository _driverRepo;
            public SetDriverWorkingStateCommandHandler(IDriversRepository driverRepo)
            {
                _driverRepo = driverRepo;
            }

            public async Task<bool> Handle(SetDriverWorkingStateCommand cmd, CancellationToken cancellationToken)
            {
                return await _driverRepo.SetDriversWorkingState(cmd.DriversId);
            }
        }
    }
}
