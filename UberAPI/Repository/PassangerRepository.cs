﻿using System;
using System.Collections.Generic;
using System.Linq;
using UberAPI.Data;
using UberAPI.Helpers;
using UberAPI.Helpers.Enums;
using UberAPI.Models;
using UberAPI.Models.Driver;
using UberAPI.Repository.IRepository;

namespace UberAPI.Repository
{
    public class PassangerRepository : IPassangerRepository
    {
        private readonly ApplicationDbContext _db;
        private List<DriversLocation> ClossestActiveDrivers {get;set;}
        public PassangerRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public List<DriversLocation> IdleVehiclesInProximity(Cordinates passangerCordinates)
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

                bool available = _db.DriversAvailability.Where(d=>d.DriversId == driver.Id).Select(u=>u.Available).FirstOrDefault();

                if (available)
                {
                    dl.Add(driver);
                    driversCount++;
                }
            }   
            ClossestActiveDrivers = dl;
            return dl;
        }
        public void SendReservationRequest(Reservation reservation)
        {
            //bool isDriverAvailable = _db.DriversAvailability.Where(d => d.DriversId == reservation.DriverId).Select(x=>x.Available).FirstOrDefault();

            //if (isDriverAvailable)
            //{
                _db.Reservations.Add(reservation);
                _db.SaveChanges();
            //}
            //else
            //{
            //    throw new Exception() { Message = "Driver is not available!" };
            //}
        }
        public bool IsThereReservationRequestPending(int passangerId)
        {
            var reservations = _db.Reservations.Where(r => r.PassangerId == passangerId && r.ReservationStatus == ReservationStatusEnum.Pending);

            if (reservations.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ReservationStatusCheck CheckRequestStatus(int passangerId)
        {
            Reservation reservation = _db.Reservations.Where(s => s.PassangerId == passangerId).FirstOrDefault();

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
                    _db.SaveChanges();
                }
            }

            resCheck.ReservationStatus = reservation.ReservationStatus;
            resCheck.ReservationTimePassed = timePassed;

            return resCheck;
        }

        public List<Reservation> ReservationHistory(int passangerId)
        {
            List<Reservation> reservationsList = _db.Reservations.Where(p=>p.PassangerId == passangerId).ToList();
            return reservationsList;
        }
    }
}
