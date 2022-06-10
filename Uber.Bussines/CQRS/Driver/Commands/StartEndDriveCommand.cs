using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Helpers.Enums;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Driver.Commands
{
    public class StartEndDriveCommand : IRequest<StartEndEnum>
    {
        public int ReservationId { get; set; }

        public class StartEndDriveCommandHandler : IRequestHandler<StartEndDriveCommand, StartEndEnum>
        {
            private readonly IDriversRepository _driverRepo;
            public StartEndDriveCommandHandler(IDriversRepository driverRepo)
            {
                _driverRepo = driverRepo;
            }

            public async Task<StartEndEnum> Handle(StartEndDriveCommand cmd, CancellationToken cancellationToken)
            {
                return await _driverRepo.StartEndDrive(cmd.ReservationId);
            }
        }
    }
}
