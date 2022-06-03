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
        public void SetDriversWorkingState(string driversId, bool availability)
        {
            //find drivers id in table
            DriversAvailability da = _db.DriversAvailability.Where(u=>u.DriversId == Convert.ToInt32(driversId)).FirstOrDefault();
            
            //if there isnt id add it and set status
            if (da == null)
            {
                DriversAvailability newDaObj = new DriversAvailability();
                newDaObj.Available = availability;
                newDaObj.DriversId = Convert.ToInt32(driversId);
                _db.DriversAvailability.Add(newDaObj);
                _db.SaveChanges();
            }
            //if there is, only set status
            else
            {
                da.Available = availability;
                _db.DriversAvailability.Update(da);
                _db.SaveChanges();
            }
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

            _db.Reservations.Update(reservation);
            _db.SaveChanges();
        }
    }
}
