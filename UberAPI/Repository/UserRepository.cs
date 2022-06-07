using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UberAPI.Data;
using UberAPI.Helpers.Constants;
using UberAPI.Models;
using UberAPI.Models.Driver;
using UberAPI.Repository.IRepository;

namespace UberAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;
        public UserRepository(ApplicationDbContext db, IOptions<AppSettings> appSettings)
        {
            _db = db;
            _appSettings = appSettings.Value;
        }
        public User Authenticate(string firstName, string lastName, string password)
        {
            var user = _db.Users
                .SingleOrDefault(x=>x.FirstName == firstName && x.LastName == lastName && x.Password == password);

            //user not found
            if (user == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = "";
            return user;
        }

        public bool IsUserUnique(string firstName, string lastName)
        {
            var user = _db.Users.SingleOrDefault(x=>x.FirstName == firstName && x.LastName == lastName);

            if (user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public User Register(User user)
        {
            User userObj = new User();

            if (user.Role == RolesConstants.Driver)
            {
                userObj = new User()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    Email = user.Email,
                    Role = user.Role,
                    VehicleBrand = user.VehicleBrand,
                    LicensePlate = user.LicensePlate,
                    PricePerKm = user.PricePerKm
                };

            }
            else
            {
                userObj = new User()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    Email = user.Email,
                    Role = user.Role,
                };
            }

            _db.Users.Add(userObj);
            _db.SaveChanges();
            userObj.Password = "";

            //when driver is registrated by default he is going to be UNAVAILABLE
            if (user.Role == RolesConstants.Driver)
            {
                DriversAvailability daObj = new DriversAvailability()
                {
                    Available = false,
                    DriversId = userObj.Id
                };

                _db.DriversAvailability.Add(daObj);
                _db.SaveChanges();
            }

            return userObj;
        }
    }
}
