using System.Collections.Generic;
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
    }
}
