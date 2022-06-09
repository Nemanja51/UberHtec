using System.Collections.Generic;
using UberAPI.Helpers;
using UberAPI.Helpers.Enums;
using UberAPI.Models;

namespace UberAPI.Repository.IRepository
{
    public interface IDriversRepository
    {
        List<User> GetAllDrivers();
        bool SetDriversWorkingState(string driversId);
        bool GetDriversWorkingState(int driversId);
        bool IsUserDriver(string driversId);
        List<Reservation> GetAllPendingReservations(int driversId);
        void AcceptOrDeclineReservation(int reservationId, ReservationDecisionEnum decision);
        void DeclineAllPendingRequests(int driversId, int reservationId);
        void DeclineReservationAfter2MinsOfPending(int reservationId);
        void SetLocation(int driversId, Cordinates newLocation);
        StartEndEnum StartEndDrive(int reservationId);
    }
}
