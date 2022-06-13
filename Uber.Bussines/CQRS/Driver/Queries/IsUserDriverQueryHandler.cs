using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Driver.Queries;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Driver.Queries
{
    public class IsUserDriverQueryHandler : IRequestHandler<IsUserDriverQuery, bool>
    {
        private readonly IDriversRepository _driverRepo;
        public IsUserDriverQueryHandler(IDriversRepository driverRepo)
        {
            _driverRepo = driverRepo;
        }

        public async Task<bool> Handle(IsUserDriverQuery query, CancellationToken cancellationToken)
        {
            return await _driverRepo.IsUserDriver(query.DriversId);
        }
    }
}
