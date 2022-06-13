using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UberAPI.Helpers;
using UberAPI.Helpers.Constants;
using System.IO;
using MediatR;
using Uber.Boundary.CQRS.Passanger.Queries;
using Uber.Boundary.Helpers;
using Uber.Boundary.CQRS.Passanger.Commands;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UberAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PassangerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PassangerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns a list of twenty idle vehicles in his proximity
        /// </summary>
        /// <returns></returns>
        [HttpPost("getclosestdrivers")]
        public IActionResult GetClosestDrivers(Cordinates passangerCordinates)
        {
            try
            {
                Log.Instance.Trace("Passanger is searching for clossest drivers...");
                var closestDriversLocations = _mediator.Send(new IdleVehiclesInProximityQuery() 
                { 
                    Cordinates = passangerCordinates 
                });
                return Ok(closestDriversLocations.Result);
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Something went wrong while Passanger was searching for clossest drivers. Error msg: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Reserve vehicle
        /// </summary>
        /// <returns></returns>
        [HttpPost("reservevehicle")]
        public IActionResult SendReservationRequest(Reservation reservation)
        {
            try
            {
                var passangerId = Convert.ToInt32(User.Identity.Name);

                //if there is already reservation request for pasanger on pending
                //passanger can not make another request
                if (_mediator.Send(new IsThereReservationRequestPendingQuery() { PassangerId = passangerId }).Result)
                {
                    //there is pending request
                    return Ok(new { message = InfoConstants.PendingReservationBePatient });
                }

                reservation.PassangerId = passangerId;
                reservation.ReservationStatus = ReservationStatusEnum.Pending;
                reservation.ReservationTime = DateTime.Now;

                _mediator.Send(new SendReservationRequestCommand() { Reservation = reservation });

                Log.Instance.Trace($"Passanger {passangerId} sent reservation request.");
                return Ok(new { message = InfoConstants.Reserved });
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Something went wrong while Passanger was sending reservation request. Error msg: " + ex.Message);
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
            try
            {
                Log.Instance.Trace($"Passanger is trying to check reservation requests.");
                var passangerId = Convert.ToInt32(User.Identity.Name);
                var check = _mediator.Send(new CheckRequestStatusQuery() { PassangerId = passangerId }).Result;

                if (check == null)
                {
                    Log.Instance.Trace($"There are no reservations.");
                    return Ok(new { message = InfoConstants.ThereAreNoReservations });
                }

                if (check.ReservationStatus == ReservationStatusEnum.Reserved)
                {
                    Log.Instance.Trace($"Reservation is accepted.");
                    return Ok(new { message = InfoConstants.AcceptedReservation });
                }
                else if (check.ReservationStatus == ReservationStatusEnum.Declined)
                {
                    Log.Instance.Trace($"Reservation is declined.");
                    return Ok(new { message = InfoConstants.DeclinedReservation });
                }

                Log.Instance.Trace($"Reservation is still pending.");
                return Ok(new { message = InfoConstants.PendingReservation });
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Something went wrong while Passanger was trying to check reservation status. Error msg: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get list of reservations for logen in passanger
        /// </summary>
        /// <returns>List of reservations</returns>
        [HttpGet("getreservationhistory")]
        public IActionResult GetReservationHistory() 
        {
            try
            {
                var passangerId = Convert.ToInt32(User.Identity.Name);
                var reservations = _mediator.Send(new ReservationHistoryQuery() { PassangerId = passangerId }).Result;

                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Rating driver from 1 to 5 with comment
        /// </summary>
        /// <param name="rateDriver"></param>
        /// <returns></returns>
        [HttpPost("submmitrate")]
        public IActionResult SubmmitRate(RateDriver rateDriver) 
        {
            try
            {
                rateDriver.PassangerId = Convert.ToInt32(User.Identity.Name);
                rateDriver.DateTimeOfRate = DateTime.Now;
                string message = _mediator.Send(new SubmmitRatingCommand() { RateDriver = rateDriver }).Result;
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Download pdf recept 
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        [HttpPost("downloadpdfrecept")]
        public FileResult GetRecept(int reservationId) 
        {
            try
            {
                //create and save pdf
                var pdf = _mediator.Send(new CreateReceptQuery() { ReservationId = reservationId }).Result;
                string pdfPath = $"Recepts/UberRecept{reservationId}.pdf";

                //if there is already pdf for reservation id dont create new
                //if there isnt, create it
                if (!System.IO.File.Exists(pdfPath))
                {
                    pdf.SaveAs(pdfPath);
                }

                //download pdf
                return DownloadPDF(pdfPath);
            }
            catch (Exception ex)
            {
                //log ex.message
                throw;
            }

            FileResult DownloadPDF(string filePath)
            {
                byte[] pdfByte = GetBytesFromFile(filePath);
                return File(pdfByte, "application/pdf", "test.pdf");
            }

            byte[] GetBytesFromFile(string fullFilePath)
            {
                // this method is limited to 2^32 byte files (4.2 GB)
                FileStream fs = null;
                try
                {
                    fs = System.IO.File.OpenRead(fullFilePath);
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                    return bytes;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }
            }
        }
    }
}
