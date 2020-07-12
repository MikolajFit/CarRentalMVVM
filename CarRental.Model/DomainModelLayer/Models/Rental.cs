using System;

namespace CarRental.Model.DomainModelLayer.Models
{
    public class Rental : AggregateRoot
    {
        public Rental(Guid id, DateTime startDateTime, Guid carId,
            Guid driverId) : base(id)
        {
            StartDateTime = startDateTime;
            CarId = carId;
            DriverId = driverId;
            Total = new Money(0);
        }

        public DateTime StartDateTime { get; set; }
        public DateTime? StopDateTime { get; set; }
        public Money Total { get; set; }
        public Guid CarId { get; set; }
        public Guid DriverId { get; set; }

        public void StopRental(DateTime stopDateTime, Money pricePerMinute, double totalMinutes, double freeMinutes,
            Money outOfBondsPenalty)
        {
            if (stopDateTime < StartDateTime)
                throw new Exception("Stop date and time is earlier than stop");
            StopDateTime = stopDateTime;
            if (totalMinutes < freeMinutes)
                totalMinutes = 0;
            else totalMinutes -= freeMinutes;
            Total = pricePerMinute.MultiplyBy(totalMinutes) + outOfBondsPenalty;
        }
    }
}