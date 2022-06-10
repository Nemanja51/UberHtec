using System.Collections.Generic;
using System.Threading.Tasks;
using Uber.Domain.Helpers;
using Uber.Domain.Helpers.Enums;
using Uber.Domain.Models;


namespace Uber.Boundary.Repository.IRepository
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
