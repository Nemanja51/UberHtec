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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UberAPI.Repository
{
    public class DriverRepository : IDriversRepository
    {
        private readonly ApplicationDbContext _db;
        public DriverRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<User>> GetAllDrivers()
        {
            return await _db.Users.Where(x => x.Role == RolesConstants.Driver).ToListAsync();
        }
        public async Task<bool> SetDriversWorkingState(string driversId)
        {
            //find drivers id in table
            var daObj = await _db.DriversAvailability.Where(u => u.DriversId == Convert.ToInt32(driversId)).FirstOrDefaultAsync();
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
            await _db.SaveChangesAsync();

            return daObj.Available;
        }
        public async Task<bool> GetDriversWorkingState(int driversId) 
        {
            bool availability = await _db.DriversAvailability
                .Where(u => u.DriversId == Convert.ToInt32(driversId))
                .Select(u=>u.Available)
                .FirstOrDefaultAsync();

            return availability;
        }
        public async Task<bool> IsUserDriver(string driversId)
        {
            string role = await _db.Users
                .Where(u=>u.Id == Convert.ToInt64(driversId))
                .Select(u=>u.Role)
                .FirstOrDefaultAsync();

            if (role.ToUpper().Equals(RolesConstants.Driver.ToUpper()))
            {
                return true;
            }
            return false;
        }
        public async Task<List<Reservation>> GetAllPendingReservations(int driversId)
        {
            var reservationStatus = await _db.Reservations
                .Where(s => s.DriverId == driversId && s.ReservationStatus == ReservationStatusEnum.Pending)
                .ToListAsync();

            return reservationStatus;
        }
        public async Task AcceptOrDeclineReservation(int reservationId, ReservationDecisionEnum decision)
        {
            var reservation = await _db.Reservations.FindAsync(reservationId);

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
            await _db.SaveChangesAsync();
        }
        public async Task DeclineAllPendingRequests(int driversId, int reservationId)
        {
            //find all pending reservations for driver exept the one that is accepted
            var reservations = _db.Reservations.Where(d=>d.DriverId == driversId 
            && d.ReservationStatus == ReservationStatusEnum.Pending && d.Id != reservationId);

            foreach (var reservation in reservations) 
            {
                reservation.ReservationStatus = ReservationStatusEnum.Declined;
            };

            _db.Reservations.UpdateRange(reservations);
            await _db.SaveChangesAsync();
        }
        public async Task DeclineReservationAfter2MinsOfPending(int reservationId)
        {
            var reservation = await _db.Reservations.Where(r => r.Id == reservationId).FirstOrDefaultAsync();
            if (reservation.ReservationStatus == ReservationStatusEnum.Pending)
            {
                reservation.ReservationStatus = ReservationStatusEnum.Declined;

                _db.Reservations.Update(reservation);
                await _db.SaveChangesAsync();
            }
        }
        public async Task SetLocation(int driversId, Cordinates newLocation)
        {
            var driversLocation = await _db.DriversLocations.Where(d=>d.DriversId == driversId).FirstOrDefaultAsync();

            if (driversLocation != null) 
            {
                driversLocation.Latitude = newLocation.Latitude;
                driversLocation.Longitude = newLocation.Longitude;

                _db.DriversLocations.Update(driversLocation);
                await _db.SaveChangesAsync();
            }
            else
            {
                //this is used if driver is setting first time his location
                DriversLocation dl = new DriversLocation() 
                { 
                    DriversId = driversId,
                    Latitude = newLocation.Latitude,
                    Longitude = newLocation.Longitude
                };

                _db.DriversLocations.Add(dl);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<StartEndEnum> StartEndDrive(int reservationId)
        {
            //check should it be start or end
            var reservationTime = await _db.ReservationTimes.Where(rt=>rt.ReservationId == reservationId).FirstOrDefaultAsync();
            if (reservationTime != null)
            {
                //end
                reservationTime.EndTime = DateTime.Now;
                _db.ReservationTimes.Update(reservationTime);
                await _db.SaveChangesAsync();

                //if ride ended that means that driver is available again
                await ChangeDriversAvailability(true, reservationId);

                return StartEndEnum.End;
            }
            else
            {
                //start
                ReservationTime rt = new ReservationTime();
                rt.ReservationId = reservationId;
                rt.StartTime = DateTime.Now;

                _db.ReservationTimes.Add(rt);
                await _db.SaveChangesAsync();

                //if ride started that means that driver is not available until ride is finished
                await ChangeDriversAvailability(false, reservationId);

                return StartEndEnum.Start;
            }

            async Task ChangeDriversAvailability(bool isDriverAvailable, int reservationId) 
            {
                //first we need to find driver id for finding wright table entity to modify
                int driversId = await _db.Reservations.Where(d => d.Id == reservationId).Select(d => d.DriverId).FirstOrDefaultAsync();
                var driversAvailiabilityObj = await _db.DriversAvailability.Where(d => d.DriversId == driversId).FirstOrDefaultAsync();

                if (isDriverAvailable)
                {
                    //set availability on true
                    driversAvailiabilityObj.Available = true;
                }
                else
                {
                    //set availability on false
                    driversAvailiabilityObj.Available = false;
                }

                _db.DriversAvailability.Update(driversAvailiabilityObj);
                await _db.SaveChangesAsync();
            }
        }
    }
}
