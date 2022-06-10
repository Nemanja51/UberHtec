using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Models.Passanger;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Passanger.Commands
{
    public class SubmmitRatingCommand : IRequest<string>
    {
        public RateDriver RateDriver { get; set; }

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
}
