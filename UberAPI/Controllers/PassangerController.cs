using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UberAPI.Helpers;
using UberAPI.Helpers.Enums;
using UberAPI.Models;
using UberAPI.Repository.IRepository;
using UberAPI.Helpers.Constants;
using UberAPI.Models.Passanger;
using System.IO;

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
            try
            {
                Log.Instance.Trace("Passanger is searching for clossest drivers...");
                var closestDriversLocations = _passangerRepo.IdleVehiclesInProximity(passangerCordinates);
                return Ok(closestDriversLocations);
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
                if (_passangerRepo.IsThereReservationRequestPending(passangerId))
                {
                    //there is pending request
                    return Ok(new { message = InfoConstants.PendingReservationBePatient });
                }

                reservation.PassangerId = passangerId;
                reservation.ReservationStatus = ReservationStatusEnum.Pending;
                reservation.ReservationTime = DateTime.Now;

                _passangerRepo.SendReservationRequest(reservation);
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
                ReservationStatusCheck check = _passangerRepo.CheckRequestStatus(passangerId);

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
                List<Reservation> reservations = _passangerRepo.ReservationHistory(passangerId);

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
                string message = _passangerRepo.SubmmitRaiting(rateDriver);
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
                var pdf = _passangerRepo.CreateRecept(reservationId);
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
