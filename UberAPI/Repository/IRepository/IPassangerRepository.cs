using System.Collections.Generic;
using UberAPI.Helpers;
using UberAPI.Helpers.Enums;
using UberAPI.Models;
using UberAPI.Models.Driver;

namespace UberAPI.Repository.IRepository
{
    public interface IPassangerRepository
    {
        public List<DriversLocation> IdleVehiclesInProximity(Cordinates passangerCordinates);
        public void SendReservationRequest(Reservation reservation);
        public ReservationStatusEnum SendReservationRequest(int passangerId);
    }
}
