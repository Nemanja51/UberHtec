using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Passanger.Queries
{
    public class IsThereReservationRequestPendingQuery : IRequest<bool>
    {
        public int PassangerId { get; set; }

        public class IsThereReservationRequestPendingQueryHandler : IRequestHandler<IsThereReservationRequestPendingQuery, bool>
        {
            private readonly IPassangerRepository _passangerRepo;
            public IsThereReservationRequestPendingQueryHandler(IPassangerRepository passangerRepo)
            {
                _passangerRepo = passangerRepo;
            }

            public async Task<bool> Handle(IsThereReservationRequestPendingQuery query, CancellationToken cancellationToken)
            {
                return await _passangerRepo.IsThereReservationRequestPending(query.PassangerId);
            }
        }
    }
}
