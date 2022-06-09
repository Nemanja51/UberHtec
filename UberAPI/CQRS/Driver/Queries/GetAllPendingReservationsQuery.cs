using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Models;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Driver.Queries
{
    public class GetAllPendingReservationsQuery : IRequest<List<Reservation>>
    {
        public int DriversId { get; set; }

        public class GetAllPendingReservationsQueryHandler : IRequestHandler<GetAllPendingReservationsQuery, List<Reservation>>
        {
            private readonly IDriversRepository _driverRepo;
            public GetAllPendingReservationsQueryHandler(IDriversRepository driverRepo)
            {
                _driverRepo = driverRepo;
            }

            public async Task<List<Reservation>> Handle(GetAllPendingReservationsQuery query, CancellationToken cancellationToken)
            {
                return await _driverRepo.GetAllPendingReservations(query.DriversId);
            }
        }
    }
}
