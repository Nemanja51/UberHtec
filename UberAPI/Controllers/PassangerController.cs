using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UberAPI.Helpers;
using UberAPI.Models;
using UberAPI.Repository.IRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UberAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PassangerController : ControllerBase
    {
        private readonly IPassangerRepository _passangerRepo;
        public PassangerController(IPassangerRepository passangerRepo)
        {
            _passangerRepo = passangerRepo;
        }

        /// <summary>
        /// Not developed 
        /// Returns a list of twenty idle vehicles in his proximity
        /// </summary>
        /// <returns></returns>
        [HttpPost("getclosestdrivers")]
        public IActionResult GetClosestDrivers(Cordinates passangerCordinates)
        {
            var closestDriversLocations = _passangerRepo.IdleVehiclesInProximity(passangerCordinates);

            return Ok(closestDriversLocations);
        }

        /// <summary>
        /// Reserve vehicle
        /// </summary>
        /// <returns></returns>
        [HttpPost("reservevehicle")]
        public IActionResult SendReservationRequest(Reservation reservation)
        {
            var passangerId = Convert.ToInt32(User.Identity.Name);

            //if there is already reservation request for pasanger on pending
            //passanger can not make another request
            if (_passangerRepo.IsThereReservationRequestPending(passangerId))
            {
                //there is pending request
                return Ok(new { message = "You have alreay pending request, please be patient!" });
            }

            try
            {
                reservation.PassangerId = passangerId;
                reservation.ReservationStatus = Helpers.Enums.ReservationStatusEnum.Pending;

                _passangerRepo.SendReservationRequest(reservation);
                return Ok(new { message = "Reserved!"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
