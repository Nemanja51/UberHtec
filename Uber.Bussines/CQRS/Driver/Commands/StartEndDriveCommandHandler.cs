using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Driver.Commands;
using Uber.Boundary.Helpers;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Driver.Commands
{

    public class StartEndDriveCommandHandler : IRequestHandler<StartEndDriveCommand, StartEndEnum>
    {
        private readonly IDriversRepository _driverRepo;
        public StartEndDriveCommandHandler(IDriversRepository driverRepo)
        {
            _driverRepo = driverRepo;
        }

        public async Task<StartEndEnum> Handle(StartEndDriveCommand cmd, CancellationToken cancellationToken)
        {
            return (StartEndEnum)await _driverRepo.StartEndDrive(cmd.ReservationId);
        }
    }

}
