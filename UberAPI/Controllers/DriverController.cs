using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using UberAPI.Helpers;
using UberAPI.Helpers.Constants;
using UberAPI.Helpers.Enums;
using UberAPI.Models;
using UberAPI.Models.Driver;
using UberAPI.Repository.IRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UberAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //(Roles = RolesConstants.Driver)
    public class DriverController : ControllerBase
    {
        private readonly IDriversRepository _driversRepo;
        public DriverController(IDriversRepository driversRepo)
        {
            _driversRepo = driversRepo;
        }

        /// <summary>
        /// We are loading 1000 drivers here 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getalldrivers")]
        public IActionResult GetAllDrivers()
        {
            var drivers = _driversRepo.GetAllDrivers();
            return Ok(drivers.Take(1000));
        }

        /// <summary>
        /// This method is used to change drivers availability 
        /// </summary>
        /// <returns></returns>
        [HttpPut("driversworkingstate")]
        public IActionResult DriversWorkingState()
        {
            var logedInUserId = User.Identity.Name;

            //other users then Driver arent authorize to use this but this is just double check
            if (!_driversRepo.IsUserDriver(logedInUserId))
            {
                return BadRequest(new { message = ErrorConstants.DriverStateError });
            }

            bool availability = _driversRepo.SetDriversWorkingState(logedInUserId);

            if (availability)
            {
                return Ok(new {  message = InfoConstants.YouAreAvailable});
            }
            else
            {
                return Ok(new { message = InfoConstants.YouAreUnavailable });
            }
        }

        /// <summary>
        /// This returns current working state of logged in driver
        /// </summary>
        /// <returns></returns>
        [HttpGet("getdriversworkingstate")]
        public IActionResult GetDriverWorkingState() 
        {
            var logedInUserId = Convert.ToInt32(User.Identity.Name);

            bool isAvailability = _driversRepo.GetDriversWorkingState(logedInUserId);

            if (isAvailability)
            {
                return Ok(new { message = InfoConstants.YouAreAvailable });
            }
            else
            {
                return Ok(new { message = InfoConstants.YouAreUnavailable });
            }
        }

        /// <summary>
        /// Get all pending reservations for loged in Driver
        /// </summary>
        /// <returns></returns>
        [HttpGet("getallpendindgreservations")]
        public IActionResult GetAllPendingReservations() 
        {
            var logedInUserId = Convert.ToInt32(User.Identity.Name);
            var reservations = _driversRepo.GetAllPendingReservations(logedInUserId);

            return Ok(reservations);
        }

        /// <summary>
        /// Accept or Decline reservation
        /// </summary>
        /// <returns></returns>
        [HttpPost("acceptordeclinereservation")]
        public IActionResult AcceptOrDeclineReservation([FromBody] Reservation reservationModel, ReservationDecisionEnum decision)
        {
            _driversRepo.AcceptOrDeclineReservation(reservationModel.Id, decision);

            if (decision == ReservationDecisionEnum.Accept) 
            {
                //if driver accepts one then all others that are pending are going to be declined
                _driversRepo.DeclineAllPendingRequests(Convert.ToInt32(User.Identity.Name), reservationModel.Id);
                return Ok(new { message = InfoConstants.YouHaveAcceptedReservation });
            }
            else if (decision == ReservationDecisionEnum.Decline)
            {
                return Ok(new { message = InfoConstants.YouHaveDeclinedReservation });
            }

            return Ok();
        }

        /// <summary>
        /// Set drivers location manualy
        /// </summary>
        /// <param name="cordinates"></param>
        /// <returns></returns>
        [HttpPost("setdriverslocation")]
        public IActionResult SetLocation(Cordinates cordinates) 
        {
            _driversRepo.SetLocation(Convert.ToInt32(User.Identity.Name), cordinates);
            return Ok();
        }

        /// <summary>
        /// Start / End drive
        /// </summary>
        /// <returns></returns>
        [HttpPost("startenddrive")]
        public IActionResult StartEndDrive(int reservationId) 
        {
            try
            {
                StartEndEnum response = _driversRepo.StartEndDrive(reservationId);

                if (response == StartEndEnum.Start)
                {
                    return Ok(new { message = "Drive has started!" });
                }
                else 
                {
                    return Ok(new { message = "Drive has ended!" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

    }
}
