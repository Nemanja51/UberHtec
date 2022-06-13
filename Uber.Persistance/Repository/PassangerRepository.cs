using System;
using System.Collections.Generic;
using System.Linq;

using IronPdf;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Uber.Domain.IRepository;
using Uber.Persistence.Data;
using Uber.Domain.Models;
using Uber.Domain.Helpers;
using Uber.Domain.Helpers.Enums;

namespace Uber.Persistence.Repository
{
    public class PassangerRepository : IPassangerRepository
    {
        private readonly ApplicationDbContext _db;
        private List<DriversLocation> ClossestActiveDrivers {get;set;}
        public PassangerRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<DriversLocation>> IdleVehiclesInProximity(Cordinates passangerCordinates)
        {
            //ordered drivers by location
            var closestDriversOrdered = _db.DriversLocations.OrderBy(x =>
               (passangerCordinates.Latitude - x.Latitude) * (passangerCordinates.Latitude - x.Latitude) 
               + (passangerCordinates.Longitude - x.Longitude) * (passangerCordinates.Longitude - x.Longitude));

            //check which one are active
            //take 20 active drivers
            List<DriversLocation> dl = new List<DriversLocation>();
            int driversCount = 0;
            foreach (var driver in closestDriversOrdered) 
            {
                if (driversCount == 20) break;

                bool available = await _db.DriversAvailability.Where(d=>d.DriversId == driver.Id).Select(u=>u.Available).FirstOrDefaultAsync();

                if (available)
                {
                    dl.Add(driver);
                    driversCount++;
                }
            }   
            ClossestActiveDrivers = dl;
            return dl;
        }
        public async Task SendReservationRequest(Reservation reservation)
        {
            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> IsThereReservationRequestPending(int passangerId)
        {
            var reservations = _db.Reservations
                .Where(r => r.PassangerId == passangerId && r.ReservationStatus == ReservationStatusEnum.Pending)
                .ToListAsync();

            if (reservations.Result.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<ReservationStatusCheck> CheckRequestStatus(int passangerId)
        {
            Reservation reservation = await _db.Reservations
                .Where(s => s.PassangerId == passangerId)
                .FirstOrDefaultAsync();

            //there are no reservations
            if (reservation == null)
            {
                return null;
            }

            TimeSpan timePassed = DateTime.Now - reservation.ReservationTime;
            ReservationStatusCheck resCheck = new ReservationStatusCheck();

            if (reservation.ReservationStatus != ReservationStatusEnum.Reserved) 
            {
                //if request is old more then 2 mins it means it is authomaticli declined
                //this logic should be in some timer that is checking and calculatin every few seconds, but for now it is ok like this
                if (timePassed.TotalSeconds > 120)
                {
                    reservation.StatusChangeTime = DateTime.Now;
                    reservation.ReservationStatus = ReservationStatusEnum.Declined;

                    _db.Reservations.Update(reservation);
                    await _db.SaveChangesAsync();
                }
            }

            resCheck.ReservationStatus = reservation.ReservationStatus;
            resCheck.ReservationTimePassed = timePassed;

            return resCheck;
        }
        public async Task<List<Reservation>> ReservationHistory(int passangerId)
        {
            List<Reservation> reservationsList = await _db.Reservations.Where(p=>p.PassangerId == passangerId).ToListAsync();
            return reservationsList;
        }
        public async Task<string> SubmmitRaiting(RateDriver rateDriver)
        {
            //first we need to check was there accepted reservation in 24h from now
            Reservation reservation = await _db.Reservations
                .Where(r => r.PassangerId == rateDriver.PassangerId
            && r.DriverId == rateDriver.DriverId && r.ReservationStatus == ReservationStatusEnum.Reserved)
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            if (reservation == null)
                return "There was no reservation for this driver!"; //there was no reservation at all for this driver from this passanger

            var reservationTime = await _db.ReservationTimes.Where(rt=>rt.ReservationId == reservation.Id).FirstOrDefaultAsync();

            if (reservationTime.EndTime < reservationTime.StartTime)
            {
                return "Drive isnt over yet! Please, leave comment when drive ends!";
            }

            var timeDiff = DateTime.Now - reservationTime.EndTime;

            if (timeDiff.TotalHours > 24)
            {
                return "The ride was more than 24 hours ago, so you can't leave a rating no more!"; // rate time limit of 24h has expired
            }

            _db.DriverRates.Add(rateDriver);
            await _db.SaveChangesAsync();

            return "You have successfully leaved a rating!";
        }
        public async Task<PdfDocument> CreateRecept(int reservationId)
        {
            var reservation = await _db.Reservations
                .Where(r=>r.Id == reservationId)
                .Include(d=>d.Driver)
                .FirstOrDefaultAsync();

            Recept recept = new Recept()
            {
                DriversFullName = reservation.Driver.FirstName + " " + reservation.Driver.LastName,
                VehicleBrand = reservation.Driver.VehicleBrand,
                LicancePlate = reservation.Driver.LicensePlate,
                Milage = CalculateMilage(reservation)
            };

            recept.TotalPrice = CalculatePrice(recept.Milage, reservation.Driver.PricePerKm);

            var htmlToPdf = new HtmlToPdf();

            StringBuilder html = new StringBuilder();
            #region Creating html
            html.Append($"<h2>HtecUber recept for ride number: <strong>{reservationId}</strong></h2>");
            html.Append($"<p></p>");
            html.Append($"</br>");

            html.Append("<label>Drivers name</label>");
            html.Append($"<p>{recept.DriversFullName}</p>");
            html.Append($"</br>");

            html.Append("<label>Vehicle brand</label>");
            html.Append($"<p>{recept.VehicleBrand}</p>");
            html.Append($"</br>");

            html.Append("<label>Licance plate</label>");
            html.Append($"<p>{recept.LicancePlate}</p>");
            html.Append($"</br>");

            html.Append("<label>Milage</label>");
            html.Append($"<p>{recept.Milage}</p>");
            html.Append($"</br>");

            html.Append("<label>Total price</label>");
            html.Append($"<p>{recept.TotalPrice}</p>");
            html.Append($"</br>");
            #endregion

            var pdfDocument = htmlToPdf.RenderHtmlAsPdf(html.ToString());

            return pdfDocument;

            double CalculateMilage(Reservation reservation) 
            {
                var d1 = reservation.PassangersCurrentLocationLatitude * (Math.PI / 180.0);
                var num1 = reservation.PassangersCurrentLocationLongitude * (Math.PI / 180.0);
                var d2 = reservation.PassangersDesiredLocationLatitude * (Math.PI / 180.0);
                var num2 = reservation.PassangersDesiredLocationLongitude * (Math.PI / 180.0) - num1;
                var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                         Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
                return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
            }
            decimal CalculatePrice(double km, decimal price) 
            {
                return (decimal)km * price;
            }
        }
    }
}
