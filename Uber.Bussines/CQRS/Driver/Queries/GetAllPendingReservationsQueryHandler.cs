using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Driver.Queries;
using Uber.Boundary.CQRS.Passanger;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Driver.Queries
{
    public class GetAllPendingReservationsQueryHandler : IRequestHandler<GetAllPendingReservationsQuery, List<ReservationResponse>>
    {
        private readonly IDriversRepository _driverRepo;
        public GetAllPendingReservationsQueryHandler(IDriversRepository driverRepo)
        {
            _driverRepo = driverRepo;
        }

        public async Task<List<ReservationResponse>> Handle(GetAllPendingReservationsQuery query, CancellationToken cancellationToken)
        {
            var result = await _driverRepo.GetAllPendingReservations(query.DriversId);

            List<ReservationResponse> responesList = new List<ReservationResponse>();

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

                responesList.Add(rr);
            }

            return responesList;
        }
    }
}
