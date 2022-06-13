using MediatR;
using System.Collections.Generic;
using Uber.Boundary.CQRS.Users;

namespace Uber.Boundary.CQRS.Driver.Queries
{
    public class GetAllDriversQuery : IRequest<List<UserResponse>>
    {
    }
}
