using System;
using DDD.Base.DomainModelLayer.Events;
using DDD.Base.DomainModelLayer.Models;

namespace DDD.CarRentalLib.DomainModelLayer.Models
{
    public class Car : AggregateRoot
    {
        private string _registrationNumber;

        public Car(Guid id, IDomainEventPublisher domainEventPublisher, string registrationNumber,
            double currentDistance, double totalDistance, decimal pricePerMinute, Guid rentalAreaId) : base(id, domainEventPublisher)
        {
            RegistrationNumber = registrationNumber;
            RentalAreaId = rentalAreaId;
            PricePerMinute = new Money(pricePerMinute);
            CurrentDistance = new Distance(currentDistance);
            TotalDistance = new Distance(totalDistance);
            Status = CarStatus.Free;
            CurrentPosition = new Position();
        }

        public Car(Guid id, IDomainEventPublisher domainEventPublisher, string registrationNumber,
            double currentDistance, double totalDistance, double currentLatitude, double currentLongitude,
            decimal pricePerMinute, Guid rentalAreaId) : this(id,
            domainEventPublisher, registrationNumber, currentDistance, totalDistance, pricePerMinute, rentalAreaId)
        {
            CurrentPosition = new Position(currentLatitude, currentLongitude);
        }

        public string RegistrationNumber
        {
            get => _registrationNumber;
            protected set
            {
                if (string.IsNullOrEmpty(value)) throw new Exception("Registration number is null or empty");
                _registrationNumber = value;
            }
        }

        public Guid RentalAreaId { get; set; }
        public Money PricePerMinute { get; protected set; }
        public Position CurrentPosition { get; protected set; }
        public Distance CurrentDistance { get; protected set; }
        public Distance TotalDistance { get; protected set; }
        public CarStatus Status { get; protected set; }

        public void ResetCurrentDistance()
        {
            CurrentDistance.Value = 0;
        }

        public void AddDistance(Distance newDistance)
        {
            CurrentDistance += newDistance;
            TotalDistance += newDistance;
        }
        public void UpdatePosition(Position newPosition)
        {
            CurrentPosition = newPosition;
        }

        public void ReserveCar()
        {
            if (Status != CarStatus.Free) throw new Exception($"Car '{RegistrationNumber}' is not free");
            Status = CarStatus.Reserved;
        }

        public void TakeCar()
        {
            if (Status != CarStatus.Free) throw new Exception($"Car '{RegistrationNumber}' is not free");
            Status = CarStatus.Taken;
        }

        public void ExitCar()
        {
            Status = CarStatus.Free;
        }
    }
}