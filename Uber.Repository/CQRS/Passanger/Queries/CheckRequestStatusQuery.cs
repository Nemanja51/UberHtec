using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Models;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Passanger.Queries
{
    public class CheckRequestStatusQuery : IRequest<ReservationStatusCheck>
    {
        public int PassangerId { get; set; }

        public class CheckRequestStatusQueryHandler : IRequestHandler<CheckRequestStatusQuery, ReservationStatusCheck>
        {
            private readonly IPassangerRepository _passangerRepo;
            public CheckRequestStatusQueryHandler(IPassangerRepository passangerRepo)
            {
                _passangerRepo = passangerRepo;
            }

            public async Task<ReservationStatusCheck> Handle(CheckRequestStatusQuery query, CancellationToken cancellationToken)
            {
                return await _passangerRepo.CheckRequestStatus(query.PassangerId);
            }
        }
    }
}
