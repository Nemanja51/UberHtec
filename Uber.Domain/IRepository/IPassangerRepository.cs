using IronPdf;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uber.Domain.Helpers;
using Uber.Domain.Models;

namespace Uber.Domain.IRepository
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
