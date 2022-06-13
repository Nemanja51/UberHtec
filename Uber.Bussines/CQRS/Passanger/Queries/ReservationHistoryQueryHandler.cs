using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Passanger;
using Uber.Boundary.CQRS.Passanger.Queries;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Passanger.Queries
{
    public class ReservationHistoryQueryHandler : IRequestHandler<ReservationHistoryQuery, List<ReservationResponse>>
    {
        private readonly IPassangerRepository _passangerRepo;
        public ReservationHistoryQueryHandler(IPassangerRepository passangerRepo)
        {
            _passangerRepo = passangerRepo;
        }

        public async Task<List<ReservationResponse>> Handle(ReservationHistoryQuery query, CancellationToken cancellationToken)
        {
            var result = await _passangerRepo.ReservationHistory(query.PassangerId);

            List<ReservationResponse> respones = new List<ReservationResponse>();

            foreach (var reservation in result)
            {
                ReservationResponse rr = new ReservationResponse()
                {
                    Id = reservation.Id,
                    DriverId = reservation.DriverId,
                    PassangerId = reservation.PassangerId,
                    ReservationStatus = (Boundary.Helpers.ReservationStatusEnum)reservation.ReservationStatus,
                    ReservationTime = reservation.ReservationTime,
                    StatusChangeTime = reservation.StatusChangeTime,
                    PassangersCurrentLocationLatitude = reservation.PassangersCurrentLocationLatitude,
                    PassangersCurrentLocationLongitude = reservation.PassangersCurrentLocationLongitude,
                    PassangersDesiredLocationLatitude = reservation.PassangersDesiredLocationLatitude,
                    PassangersDesiredLocationLongitude = reservation.PassangersDesiredLocationLongitude
                };

                respones.Add(rr);
            }

            return respones;
        }
    }
}
