using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Driver.Commands;
using Uber.Domain.Helpers;
using Uber.Domain.IRepository;

namespace UberAPI.CQRS.Driver.Commands
{
    public class SetLocationCommandHandler : IRequestHandler<SetLocationCommand>
    {
        private readonly IDriversRepository _driverRepo;
        public SetLocationCommandHandler(IDriversRepository driverRepo)
        {
            _driverRepo = driverRepo;
        }

        public async Task<Unit> Handle(SetLocationCommand cmd, CancellationToken cancellationToken)
        {
            Cordinates cordinates = new Cordinates() 
            { 
                Latitude = cmd.NewCordinates.Latitude,
                Longitude = cmd.NewCordinates.Longitude
            };

            await _driverRepo.SetLocation(cmd.DriversId, cordinates);
            return Unit.Value;
        }
    }
}
