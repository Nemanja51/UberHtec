using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Passanger.Queries;
using Uber.Boundary.CQRS.Passanger;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Passanger.Queries
{
    public class CheckRequestStatusQueryHandler : IRequestHandler<CheckRequestStatusQuery, ReservationStatusCheckResponse>
    {
        private readonly IPassangerRepository _passangerRepo;
        public CheckRequestStatusQueryHandler(IPassangerRepository passangerRepo)
        {
            _passangerRepo = passangerRepo;
        }

        public async Task<ReservationStatusCheckResponse> Handle(CheckRequestStatusQuery query, CancellationToken cancellationToken)
        {
            var result = await _passangerRepo.CheckRequestStatus(query.PassangerId);
            return new ReservationStatusCheckResponse()
            {
                ReservationStatus = (Boundary.Helpers.ReservationStatusEnum)result.ReservationStatus,
                ReservationTimePassed = result.ReservationTimePassed
            };
        }
    }

}
