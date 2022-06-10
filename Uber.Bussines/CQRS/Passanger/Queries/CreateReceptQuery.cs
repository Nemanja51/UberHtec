using IronPdf;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Passanger.Queries
{
    public class CreateReceptQuery : IRequest<PdfDocument>
    {
        public int ReservationId { get; set; }
        public class CreateReceptQueryHandler : IRequestHandler<CreateReceptQuery, PdfDocument>
        {
            private readonly IPassangerRepository _passangerRepo;
            public CreateReceptQueryHandler(IPassangerRepository passangerRepo)
            {
                _passangerRepo = passangerRepo;
            }

            public async Task<PdfDocument> Handle(CreateReceptQuery query, CancellationToken cancellationToken)
            {
                return await _passangerRepo.CreateRecept(query.ReservationId);
            }
        }
    }
}
