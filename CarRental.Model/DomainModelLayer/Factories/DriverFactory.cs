using System;
using CarRental.Model.DomainModelLayer.Models;

namespace CarRental.Model.DomainModelLayer.Factories
{
    public class DriverFactory
    {
        public Driver Create(Guid id, string licenseNumber, string firstName, string lastName)
        {
            return new Driver(id, licenseNumber, firstName, lastName);
        }
    }
}