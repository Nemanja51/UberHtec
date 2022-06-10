using MediatR;
using System.Threading;
using System.Threading.Tasks;
namespace Uber.Boundary.CQRS.Users.Queries
{
    public class IsUserUniqueQuery : IRequest<bool>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
    }
}
