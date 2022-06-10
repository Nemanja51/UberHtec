using System.Threading.Tasks;
using Uber.Domain.Models;

namespace Uber.Boundary.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<bool> IsUserUnique(string firstName, string lastName);
        Task<User> Authenticate(string firstName, string lastName, string password);
        Task<User> Register(User user);
    }
}
