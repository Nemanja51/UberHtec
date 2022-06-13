using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Passanger.Commands;
using Uber.Domain.IRepository;
using Uber.Domain.Models;

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
            RateDriver rate = new RateDriver() 
            {
                Id = query.RateDriver.Id,
                DriverId = query.RateDriver.DriverId,
                PassangerId = query.RateDriver.PassangerId,
                Rate = query.RateDriver.Rate,
                Comment = query.RateDriver.Comment,
                DateTimeOfRate = query.RateDriver.DateTimeOfRate
            };

            return await _passangerRepo.SubmmitRaiting(rate);
        }
    }
}
