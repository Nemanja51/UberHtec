using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [HttpPost("driversworkingstate")]
        public IActionResult DriversWorkingState([FromBody] DriversAvailability da)
        {
            var logedInUserId = User.Identity.Name;

            //other users then Driver arent authorize to use this but this is just double check
            if (!_driversRepo.IsUserDriver(logedInUserId))
            {
                return BadRequest(new { message = ErrorConstants.DriverStateError });
            }

            _driversRepo.SetDriversWorkingState(logedInUserId, da.Available);
            return Ok();
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
            //var logedInUserId = Convert.ToInt32(User.Identity.Name);
            //var reservations = _driversRepo.GetAllPendingReservations(logedInUserId);

            _driversRepo.AcceptOrDeclineReservation(reservationModel.Id, decision);

            return Ok(reservationModel);
        }


    }
}
