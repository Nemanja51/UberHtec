using IronPdf;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Passanger.Queries;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Passanger.Queries
{
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
