using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Passanger;
using Uber.Boundary.CQRS.Passanger.Queries;
using Uber.Domain.Helpers;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Passanger.Queries
{
    public class IdleVehiclesInProximityQueryHandler : IRequestHandler<IdleVehiclesInProximityQuery, List<DriversLocationResponse>>
    {
        private readonly IPassangerRepository _passangerRepo;
        public IdleVehiclesInProximityQueryHandler(IPassangerRepository passangerRepo)
        {
            _passangerRepo = passangerRepo;
        }

        public async Task<List<DriversLocationResponse>> Handle(IdleVehiclesInProximityQuery query, CancellationToken cancellationToken)
        {
            Cordinates cordinates = new Cordinates() 
            { 
                Latitude = query.Cordinates.Latitude,
                Longitude = query.Cordinates.Longitude
            };

            var result = await _passangerRepo.IdleVehiclesInProximity(cordinates);

            List <DriversLocationResponse> dlList = new List<DriversLocationResponse>();

            foreach (var dl in result)
            {
                DriversLocationResponse dlr = new DriversLocationResponse() 
                {
                    Id = dl.Id,
                    Latitude = dl.Latitude,
                    Longitude = dl.Longitude,
                    DriversId = dl.DriversId
                };

                dlList.Add(dlr);
            }

            return dlList;
        }
    }
}
