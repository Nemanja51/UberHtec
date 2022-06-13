using IronPdf;
using MediatR;

namespace Uber.Boundary.CQRS.Passanger.Queries
{
    public class CreateReceptQuery : IRequest<PdfDocument>
    {
        public int ReservationId { get; set; }
    }
}
