using System.Collections.Generic;
using UberAPI.Helpers;
using UberAPI.Helpers.Enums;
using UberAPI.Models;

namespace UberAPI.Repository.IRepository
{
    public interface IDriversRepository
    {
        List<User> GetAllDrivers();
        void SetDriversWorkingState(string driversId, bool availability);
        bool IsUserDriver(string driversId);
        List<Reservation> GetAllPendingReservations(int driversId);
        void AcceptOrDeclineReservation(int reservationId, ReservationDecisionEnum decision);
        void DeclineAllPendingRequests(int driversId, int reservationId);
        void SetLocation(int driversId, Cordinates newLocation);
        StartEndEnum StartEndDrive(int reservationId);
    }
}
