using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Uber.Boundary.CQRS.Users.Queries;
using Uber.Domain.IRepository;

namespace Uber.Bussines.CQRS.Users.Queries
{

    public class IsUserUniqueQueryHandler : IRequestHandler<IsUserUniqueQuery, bool>
    {
        private readonly IUserRepository _userRepository;

        public IsUserUniqueQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(IsUserUniqueQuery query, CancellationToken cancellationToken)
        {
            return await _userRepository.IsUserUnique(query.FirstName, query.LastName);
        }
    }

}
