using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UberAPI.Models;
using UberAPI.Repository.IRepository;

namespace UberAPI.CQRS.Users.Queries
{
    public class AuthenticateQuery : IRequest<User>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public class AuthenticateQueryHandler : IRequestHandler<AuthenticateQuery, User>
        {
            private readonly IUserRepository _userRepository;

            public AuthenticateQueryHandler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<User> Handle(AuthenticateQuery query, CancellationToken cancellationToken)
            {
                return await _userRepository.Authenticate(query.FirstName, query.LastName, query.Password);
            }
        }
    }
}
