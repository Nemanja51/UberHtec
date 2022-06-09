using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Helpers;
using UberAPI.Models.Driver;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Passanger.Queries
{
    public class IdleVehiclesInProximityQuery : IRequest<List<DriversLocation>>
    {
        public Cordinates Cordinates { get; set; }

        public class IdleVehiclesInProximityQueryHandler : IRequestHandler<IdleVehiclesInProximityQuery, List<DriversLocation>>
        {
            private readonly IPassangerRepository _passangerRepo;
            public IdleVehiclesInProximityQueryHandler(IPassangerRepository passangerRepo)
            {
                _passangerRepo = passangerRepo;
            }

            public async Task<List<DriversLocation>> Handle(IdleVehiclesInProximityQuery query, CancellationToken cancellationToken)
            {
                return await _passangerRepo.IdleVehiclesInProximity(query.Cordinates);
            }
        }
    }
}
