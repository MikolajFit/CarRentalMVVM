using System;
using CarRental.Model.DomainModelLayer.Models;
using CarRental.Model.DomainModelLayer.Policies;

namespace CarRental.Model.ApplicationLayer.DTOs
{
    public class DriverDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public DriverStatus DriverStatus { get; set; }
        public PoliciesEnum FreeMinutesPolicy { get; set; }
    }
}
