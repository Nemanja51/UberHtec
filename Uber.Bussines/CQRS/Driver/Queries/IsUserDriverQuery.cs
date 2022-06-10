using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Driver.Queries
{
    public class IsUserDriverQuery : IRequest<bool>
    {
        public string DriversId { get; set; }

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
}
