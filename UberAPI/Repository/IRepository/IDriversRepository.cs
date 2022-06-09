using System.Collections.Generic;
using System.Threading.Tasks;
using UberAPI.Helpers;
using UberAPI.Helpers.Enums;
using UberAPI.Models;

namespace UberAPI.Repository.IRepository
{
    public interface IDriversRepository
    {
        Task<List<User>> GetAllDrivers();
        Task<bool> SetDriversWorkingState(string driversId);
        Task<bool> GetDriversWorkingState(int driversId);
        Task<bool> IsUserDriver(string driversId);
        Task<List<Reservation>> GetAllPendingReservations(int driversId);
        Task AcceptOrDeclineReservation(int reservationId, ReservationDecisionEnum decision);
        Task DeclineAllPendingRequests(int driversId, int reservationId);
        Task DeclineReservationAfter2MinsOfPending(int reservationId);
        Task SetLocation(int driversId, Cordinates newLocation);
        Task<StartEndEnum> StartEndDrive(int reservationId);
    }
}
