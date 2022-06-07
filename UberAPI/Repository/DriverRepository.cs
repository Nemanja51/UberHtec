using System.Collections.Generic;
using System.Linq;
using UberAPI.Data;
using UberAPI.Models;
using UberAPI.Repository.IRepository;
using UberAPI.Helpers.Constants;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using UberAPI.Models.Driver;
using UberAPI.Helpers.Enums;
using UberAPI.Helpers;

namespace UberAPI.Repository
{
    public class DriverRepository : IDriversRepository
    {
        private readonly ApplicationDbContext _db;
        public DriverRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public List<User> GetAllDrivers()
        {
            return _db.Users.Where(x => x.Role == RolesConstants.Driver).ToList();
        }
        public bool SetDriversWorkingState(string driversId)
        {
            //find drivers id in table
            var daObj = _db.DriversAvailability.Where(u => u.DriversId == Convert.ToInt32(driversId)).FirstOrDefault();
            bool availability = daObj.Available;

            if (availability) 
            { 
                daObj.Available = false;
            }
            else
            {
                daObj.Available = true;
            }

            _db.DriversAvailability.Update(daObj);
            _db.SaveChanges();

            return daObj.Available;
            
        }
        public bool GetDriversWorkingState(int driversId) 
        {
            bool availability = _db.DriversAvailability.Where(u => u.DriversId == Convert.ToInt32(driversId)).Select(u=>u.Available).FirstOrDefault();
            return availability;
        }
        public bool IsUserDriver(string driversId)
        {
            string role = _db.Users.Where(u=>u.Id == Convert.ToInt64(driversId)).Select(u=>u.Role).FirstOrDefault();
            if (role.ToUpper().Equals(RolesConstants.Driver.ToUpper()))
            {
                return true;
            }
            return false;
        }
        public List<Reservation> GetAllPendingReservations(int driversId)
        {
            var reservationStatus = _db.Reservations.Where(s => s.DriverId == driversId && s.ReservationStatus == ReservationStatusEnum.Pending).ToList();
            return reservationStatus;
        }
        public void AcceptOrDeclineReservation(int reservationId, ReservationDecisionEnum decision)
        {
            var reservation = _db.Reservations.Find(reservationId);

            switch (decision) 
            {
                case ReservationDecisionEnum.Accept:
                    reservation.ReservationStatus = ReservationStatusEnum.Reserved;
                    break;
                case ReservationDecisionEnum.Decline:
                    reservation.ReservationStatus = ReservationStatusEnum.Declined;
                    break;
            }

            reservation.StatusChangeTime = DateTime.Now;

            _db.Reservations.Update(reservation);
            _db.SaveChanges();
        }
        public void DeclineAllPendingRequests(int driversId, int reservationId)
        {
            //find all pending reservations for driver exept the one that is accepted
            var reservations = _db.Reservations.Where(d=>d.DriverId == driversId 
            && d.ReservationStatus == ReservationStatusEnum.Pending && d.Id != reservationId);

            foreach (var reservation in reservations) 
            {
                reservation.ReservationStatus = ReservationStatusEnum.Declined;
            };

            _db.Reservations.UpdateRange(reservations);
            _db.SaveChanges();

        }
        public void SetLocation(int driversId, Cordinates newLocation)
        {
            var driversLocation = _db.DriversLocations.Where(d=>d.DriversId == driversId).FirstOrDefault();
            driversLocation.Latitude = newLocation.Latitude;
            driversLocation.Longitude = newLocation.Longitude;

            _db.DriversLocations.Update(driversLocation);
            _db.SaveChanges();
        }
        public StartEndEnum StartEndDrive(int reservationId)
        {
            //check should it be start or end
            var reservationTime = _db.ReservationTimes.Where(rt=>rt.ReservationId == reservationId).FirstOrDefault();
            if (reservationTime != null)
            {
                //end
                reservationTime.EndTime = DateTime.Now;
                return StartEndEnum.End;
            }
            else
            {
                //start
                reservationTime.Id = reservationId;
                reservationTime.StartTime = DateTime.Now;
                return StartEndEnum.Start;
            }

        }
    }
}
