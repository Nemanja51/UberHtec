using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UberAPI.Helpers;
using UberAPI.Helpers.Enums;
using UberAPI.Models;
using UberAPI.Repository.IRepository;
using UberAPI.Helpers.Constants;

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
                return Ok(new { message = InfoConstants.PendingReservationBePatient });
            }

            try
            {
                reservation.PassangerId = passangerId;
                reservation.ReservationStatus = ReservationStatusEnum.Pending;
                reservation.ReservationTime = DateTime.Now;

                _passangerRepo.SendReservationRequest(reservation);
                return Ok(new { message = InfoConstants.Reserved });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Checking reservation request status and time from sending reservation request
        /// </summary>
        /// <returns></returns>
        [HttpGet("checkreservationstatus")]
        public IActionResult CheckReservationRequest() 
        {
            var passangerId = Convert.ToInt32(User.Identity.Name);
            ReservationStatusCheck check = _passangerRepo.CheckRequestStatus(passangerId);

            if (check == null)
            {
                return Ok(new { message = InfoConstants.ThereAreNoReservations });
            }

            if (check.ReservationStatus == ReservationStatusEnum.Reserved)
            {
                return Ok(new { message = InfoConstants.AcceptedReservation });
            }
            else if (check.ReservationStatus == ReservationStatusEnum.Declined)
            {
                return Ok(new { message = InfoConstants.DeclinedReservation });
            }
            return Ok(new { message = InfoConstants.PendingReservation });
        }

        /// <summary>
        /// Get list of reservations for logen in passanger
        /// </summary>
        /// <returns>List of reservations</returns>
        [HttpGet("getreservationhistory")]
        public IActionResult GetReservationHistory() 
        {
            var passangerId = Convert.ToInt32(User.Identity.Name);
            List<Reservation> reservations = _passangerRepo.ReservationHistory(passangerId);

            return Ok(reservations);
        }


    }
}
