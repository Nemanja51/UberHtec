using IronPdf;
using System.Collections.Generic;
using System.Threading.Tasks;
using UberAPI.Helpers;
using UberAPI.Helpers.Enums;
using UberAPI.Models;
using UberAPI.Models.Driver;
using UberAPI.Models.Passanger;

namespace UberAPI.Repository.IRepository
{
    public interface IPassangerRepository
    {
        public Task<List<DriversLocation>> IdleVehiclesInProximity(Cordinates passangerCordinates);
        public Task SendReservationRequest(Reservation reservation);
        public Task<bool> IsThereReservationRequestPending(int passangerId);
        public Task<ReservationStatusCheck> CheckRequestStatus(int passangerId);
        public Task<List<Reservation>> ReservationHistory(int passangerId);
        public Task<string> SubmmitRaiting(RateDriver rateDriver);
        public Task<PdfDocument> CreateRecept(int reservationId);
    }
}
