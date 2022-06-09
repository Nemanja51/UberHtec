using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UberAPI.CQRS.Users.Commands;
using UberAPI.CQRS.Users.Queries;
using UberAPI.Helpers.Constants;
using UberAPI.Models;
using UberAPI.Repository.IRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UberAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Authenticate/log in user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User userModel) 
        {
            var user = _mediator.Send( new AuthenticateQuery() { FirstName = userModel.FirstName, LastName = userModel.LastName, Password = userModel.Password });

            //if we get null that means that there is no user with this credentials
            if (user == null)
            {
                return BadRequest(new { message = ErrorConstants.IncorectCredentials });
            }

            return Ok(user);
        }


        /// <summary>
        /// Registrating new user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] User userModel) 
        {
            //checking are the Required fields populated
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = ErrorConstants.ModelStateNotValid });
            }

            //checking is there already registraded user with same credencials
            var isUsernameUnique = _mediator.Send(new IsUserUniqueQuery() 
            { 
                FirstName = userModel.FirstName,
                LastName = userModel.LastName 
            });

            if (!isUsernameUnique.Result)
            {
                return BadRequest(new { message = ErrorConstants.UserAlreadyExist });
            }

            //double check of role, because based on role we are populating db
            //so if there is no role entered = BadRequest,
            //if someone entered role other than 3 existing ones = BadRequest,
            if (string.IsNullOrEmpty(userModel.Role))
            {
                return BadRequest(new { message = ErrorConstants.RoleRequired });
            }
            else if (userModel.Role.ToUpper() != RolesConstants.Admin.ToUpper() && userModel.Role.ToUpper() != RolesConstants.Driver.ToUpper() && userModel.Role.ToUpper() != RolesConstants.Passanger.ToUpper())
            {
                return BadRequest(new { message = ErrorConstants.ExistingRoles });
            }

            //if role is Driver, we need to make sure user entered other fields beside required ones from model
            if (userModel.Role.ToUpper() == RolesConstants.Driver.ToUpper())
            {
                if (string.IsNullOrEmpty(userModel.LicensePlate) && string.IsNullOrEmpty(userModel.VehicleBrand))
                {
                    return BadRequest(new { message = ErrorConstants.RequerdFieldsForDriver });
                }
            }

            //if everything is fine so far we are registrating user
            //if we get null that means something went wrong
            var user = _mediator.Send(new RegisterCommand() 
            { 
                Email = userModel.Email, 
                Password = userModel.Password, 
                LastName = userModel.LastName, 
                FirstName = userModel.FirstName, 
                LicensePlate = userModel.LicensePlate, 
                PricePerKm = userModel.PricePerKm, 
                Role = userModel.Role, 
                VehicleBrand = userModel.VehicleBrand 
            });

            if (user == null)
            {
                return BadRequest(new { message = ErrorConstants.ErrorWhileRegistrating });
            }

            return Ok();
        }
    }
}
