using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.CarRentalLib.ApplicationLayer.DTOs
{
    public class RentalDTO
    {
        public Guid Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? StopDateTime { get; set; }
        public decimal Total { get; set; }
        public Guid CarId { get; set; }
        public string RegistrationNumber { get; set; }
        public Guid DriverId { get; set; }
        public string DriverName { get; set; }

    }
}
