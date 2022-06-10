using MediatR;
using Uber.Boundary.CQRS.Users;

namespace Uber.Bussines.CQRS.Users.Queries
{
    public class AuthenticateQuery : IRequest<UserResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}
