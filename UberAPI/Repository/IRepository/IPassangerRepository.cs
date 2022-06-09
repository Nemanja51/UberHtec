using IronPdf;
using System.Collections.Generic;
using UberAPI.Helpers;
using UberAPI.Helpers.Enums;
using UberAPI.Models;
using UberAPI.Models.Driver;
using UberAPI.Models.Passanger;

namespace UberAPI.Repository.IRepository
{
    public interface IPassangerRepository
    {
        public List<DriversLocation> IdleVehiclesInProximity(Cordinates passangerCordinates);
        public void SendReservationRequest(Reservation reservation);
        public bool IsThereReservationRequestPending(int passangerId);
        public ReservationStatusCheck CheckRequestStatus(int passangerId);
        public List<Reservation> ReservationHistory(int passangerId);
        public string SubmmitRaiting(RateDriver rateDriver);
        public PdfDocument CreateRecept(int reservationId);
    }
}
