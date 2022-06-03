using UberAPI.Models;

namespace UberAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUserUnique(string firstName, string lastName);
        User Authenticate(string firstName, string lastName, string password);
        User Register(User user);
    }
}
