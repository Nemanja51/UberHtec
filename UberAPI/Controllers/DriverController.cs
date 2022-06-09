using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using UberAPI.CQRS.Driver.Commands;
using UberAPI.CQRS.Driver.Queries;
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
        private readonly IMediator _mediator;
        public DriverController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// We are loading 1000 drivers here 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getalldrivers")]
        public IActionResult GetAllDrivers()
        {
            try
            {
                var drivers = _mediator.Send(new GetAllDriversQuery());
                return Ok(drivers.Result.Take(1000));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// This method is used to change drivers availability 
        /// </summary>
        /// <returns></returns>
        [HttpPut("driversworkingstate")]
        public IActionResult DriversWorkingState()
        {
            try
            {
                var logedInUserId = User.Identity.Name;

                //other users then Driver arent authorize to use this but this is just double check
                if (!_mediator.Send(new IsUserDriverQuery() { DriversId = logedInUserId }).Result)
                {
                    return BadRequest(new { message = ErrorConstants.DriverStateError });
                }

                bool availability = _mediator.Send(new SetDriverWorkingStateCommand() 
                { 
                    DriversId = logedInUserId 
                }).Result;

                if (availability)
                {
                    return Ok(new { message = InfoConstants.YouAreAvailable });
                }
                else
                {
                    return Ok(new { message = InfoConstants.YouAreUnavailable });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// This returns current working state of logged in driver
        /// </summary>
        /// <returns></returns>
        [HttpGet("getdriversworkingstate")]
        public IActionResult GetDriverWorkingState() 
        {
            try
            {
                var logedInUserId = Convert.ToInt32(User.Identity.Name);

                bool isAvailability = _mediator.Send(new GetDriversWorkingStateQuery() { DriversId = logedInUserId}).Result;

                if (isAvailability)
                {
                    return Ok(new { message = InfoConstants.YouAreAvailable });
                }
                else
                {
                    return Ok(new { message = InfoConstants.YouAreUnavailable });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all pending reservations for loged in Driver
        /// </summary>
        /// <returns></returns>
        [HttpGet("getallpendindgreservations")]
        public IActionResult GetAllPendingReservations() 
        {
            try
            {
                var logedInUserId = Convert.ToInt32(User.Identity.Name);
                var reservations = _mediator.Send(new GetAllPendingReservationsQuery() { DriversId = logedInUserId });

                return Ok(reservations.Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Accept or Decline reservation
        /// </summary>
        /// <returns></returns>
        [HttpPost("acceptordeclinereservation")]
        public IActionResult AcceptOrDeclineReservation(int reservationId, ReservationDecisionEnum decision)
        {
            //setting timer in case that reservation goes on pending longer then 2 mins
            TimerHelper.SetTimer(reservationId);

            try
            {
                _mediator.Send(new AcceptOrDeclineReservationCommand() { ReservationId = reservationId, Decision = decision });

                if (decision == ReservationDecisionEnum.Accept)
                {
                    //if driver accepts one then all others that are pending are going to be declined
                    _mediator.Send(new DeclineAllPendingRequestsCommand() { DriversId = Convert.ToInt32(User.Identity.Name), ReservationId = reservationId });
                    return Ok(new { message = InfoConstants.YouHaveAcceptedReservation });
                }
                else if (decision == ReservationDecisionEnum.Decline)
                {
                    return Ok(new { message = InfoConstants.YouHaveDeclinedReservation });
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Set drivers location manualy
        /// </summary>
        /// <param name="cordinates"></param>
        /// <returns></returns>
        [HttpPost("setdriverslocation")]
        public IActionResult SetLocation(Cordinates cordinates) 
        {
            try
            {
                _mediator.Send(new SetLocationCommand() 
                { 
                    DriversId = Convert.ToInt32(User.Identity.Name),
                    NewCordinates = cordinates 
                });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
                StartEndEnum response = _mediator.Send(new StartEndDriveCommand() 
                { 
                    ReservationId = reservationId 
                }).Result;

                if (response == StartEndEnum.Start)
                {
                    return Ok(new { message = InfoConstants.DriveHasStarted });
                }
                else 
                {
                    //TODO: when drive is ended we need to print PDF recept
                    return Ok(new { message = InfoConstants.DriveHasEnded });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        //this should be customized with Authorization attribute and Role prop, but I cant find how
        private void ChackAuthorization() 
        {
            //if user isnt driver return NotAuthorized()
        }
    }
}
