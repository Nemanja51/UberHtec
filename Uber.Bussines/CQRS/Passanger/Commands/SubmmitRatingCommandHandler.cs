using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Passanger.Commands;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Passanger.Commands
{
    public class SubmmitRatingCommandHandler : IRequestHandler<SubmmitRatingCommand, string>
    {
        private readonly IPassangerRepository _passangerRepo;
        public SubmmitRatingCommandHandler(IPassangerRepository passangerRepo)
        {
            _passangerRepo = passangerRepo;
        }

        public async Task<string> Handle(SubmmitRatingCommand query, CancellationToken cancellationToken)
        {
            return await _passangerRepo.SubmmitRaiting(query.RateDriver);
        }
    }
}
