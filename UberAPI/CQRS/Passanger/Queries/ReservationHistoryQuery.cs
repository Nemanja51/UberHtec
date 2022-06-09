using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Models;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Passanger.Queries
{
    public class ReservationHistoryQuery : IRequest<List<Reservation>>
    {
        public int PassangerId { get; set; }

        public class ReservationHistoryQueryHandler : IRequestHandler<ReservationHistoryQuery, List<Reservation>>
        {
            private readonly IPassangerRepository _passangerRepo;
            public ReservationHistoryQueryHandler(IPassangerRepository passangerRepo)
            {
                _passangerRepo = passangerRepo;
            }

            public async Task<List<Reservation>> Handle(ReservationHistoryQuery query, CancellationToken cancellationToken)
            {
                return await _passangerRepo.ReservationHistory(query.PassangerId);
            }
        }
    }
}
