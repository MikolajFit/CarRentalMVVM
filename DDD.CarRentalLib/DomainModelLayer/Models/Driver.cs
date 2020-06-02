﻿using System;
using DDD.Base.DomainModelLayer.Events;
using DDD.Base.DomainModelLayer.Models;
using DDD.CarRentalLib.DomainModelLayer.Interfaces;
using DDD.CarRentalLib.DomainModelLayer.Policies;

namespace DDD.CarRentalLib.DomainModelLayer.Models
{
    public class Driver : AggregateRoot
    {
        private string _firstName;
        private string _lastName;
        private string _licenseNumber;
        private IFreeMinutesPolicy _policy;

        public Driver(Guid id, IDomainEventPublisher domainEventPublisher, string licenseNumber, string firstName,
            string lastName) : base(id, domainEventPublisher)
        {
            LicenseNumber = licenseNumber;
            FirstName = firstName;
            LastName = lastName;
            if (FirstName.EndsWith('a')) RegisterPolicy(new StandardFreeMinutesPolicy());
            else RegisterPolicy(new VipFreeMinutesPolicy());
        }

        public string LicenseNumber
        {
            get => _licenseNumber;
            protected set
            {
                if (string.IsNullOrEmpty(value)) throw new Exception("License number is null or empty");
                _licenseNumber = value;
            }
        }

        public string FirstName
        {
            get => _firstName;
            protected set
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

        public double CalculateFreeMinutes(double total) => _policy?.CalculateFreeMinutes(total) ?? 0;

        public void RegisterPolicy(IFreeMinutesPolicy policy)
        {
            _policy = policy;
        }
    }
}