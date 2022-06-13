using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Driver.Commands;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Driver.Commands
{
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
