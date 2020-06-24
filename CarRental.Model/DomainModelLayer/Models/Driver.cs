using System;
using CarRental.Model.DomainModelLayer.Interfaces;
using CarRental.Model.DomainModelLayer.Policies;
using DDD.Base.DomainModelLayer.Models;

namespace CarRental.Model.DomainModelLayer.Models
{
    public class Driver : AggregateRoot
    {
        private string _firstName;
        private string _lastName;
        private string _licenseNumber;

        public Driver(Guid id, string licenseNumber, string firstName,
            string lastName) : base(id)
        {
            LicenseNumber = licenseNumber;
            FirstName = firstName;
            LastName = lastName;
            DriverStatus = DriverStatus.Active;
            if (FirstName.EndsWith("a")) RegisterPolicy(new StandardFreeMinutesPolicy());
            else RegisterPolicy(new VipFreeMinutesPolicy());
        }

        public IFreeMinutesPolicy FreeMinutesPolicy { get; protected set; }
        public DriverStatus DriverStatus { get; set; }

        public string LicenseNumber
        {
            get => _licenseNumber;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new Exception("License number is null or empty");
                _licenseNumber = value;
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new Exception("First name is null or empty");
                _firstName = value;
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new Exception("Registration number is null or empty");
                _lastName = value;
            }
        }

        public double CalculateFreeMinutes(double total)
        {
            return FreeMinutesPolicy?.CalculateFreeMinutes(total) ?? 0;
        }

        public void RegisterPolicy(IFreeMinutesPolicy policy)
        {
            FreeMinutesPolicy = policy;
        }
    }
}