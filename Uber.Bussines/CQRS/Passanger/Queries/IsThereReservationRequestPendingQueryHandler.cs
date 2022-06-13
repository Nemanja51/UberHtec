using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Passanger.Queries;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Passanger.Queries
{
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
