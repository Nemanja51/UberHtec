using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Passanger.Commands;
using Uber.Domain.IRepository;
using Uber.Domain.Models;

namespace Uber.Bussines.CQRS.Passanger.Commands
{
    public class SendReservationRequestCommandHandler : IRequestHandler<SendReservationRequestCommand>
    {
        private readonly IPassangerRepository _passangerRepo;
        public SendReservationRequestCommandHandler(IPassangerRepository passangerRepo)
        {
            _passangerRepo = passangerRepo;
        }

        public async Task<Unit> Handle(SendReservationRequestCommand cmd, CancellationToken cancellationToken)
        {
            Reservation reservation = new Reservation() 
            {
                Id = cmd.Reservation.Id,
                DriverId = cmd.Reservation.DriverId,
                PassangerId = cmd.Reservation.PassangerId,
                ReservationStatus = (Domain.Helpers.Enums.ReservationStatusEnum)cmd.Reservation.ReservationStatus,
                ReservationTime = cmd.Reservation.ReservationTime,
                StatusChangeTime = cmd.Reservation.StatusChangeTime,
                PassangersCurrentLocationLatitude = cmd.Reservation.PassangersCurrentLocationLatitude,
                PassangersCurrentLocationLongitude = cmd.Reservation.PassangersCurrentLocationLongitude,
                PassangersDesiredLocationLatitude = cmd.Reservation.PassangersDesiredLocationLatitude,
                PassangersDesiredLocationLongitude = cmd.Reservation.PassangersDesiredLocationLongitude
            };

            await _passangerRepo.SendReservationRequest(reservation);
            return Unit.Value;
        }
    }
}
