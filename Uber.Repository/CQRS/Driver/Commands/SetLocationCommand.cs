using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Helpers;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Driver.Commands
{
    public class SetLocationCommand : IRequest
    {
        public int DriversId { get; set; }
        public Cordinates NewCordinates { get; set; }

        public class SetLocationCommandHandler : IRequestHandler<SetLocationCommand>
        {
            private readonly IDriversRepository _driverRepo;
            public SetLocationCommandHandler(IDriversRepository driverRepo)
            {
                _driverRepo = driverRepo;
            }

            public async Task<Unit> Handle(SetLocationCommand cmd, CancellationToken cancellationToken)
            {
                await _driverRepo.SetLocation(cmd.DriversId, cmd.NewCordinates);
                return Unit.Value;
            }
        }
    }
}
