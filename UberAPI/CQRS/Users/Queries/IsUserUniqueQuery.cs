using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Users.Queries
{
    public class IsUserUniqueQuery : IRequest<bool>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

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
}
