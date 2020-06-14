using System;
using CarRental.Model.DomainModelLayer.Models;

namespace CarRental.Model.ApplicationLayer.DTOs
{
    public class CarDTO
    {
        public Guid Id { get; set; }
        public string RegistrationNumber { get; set; }
        public double CurrentLatitude { get; set; }
        public double CurrentLongitude { get; set; }
        public double CurrentDistance { get; set; }
        public double TotalDistance { get; set; }
        public CarStatus Status { get; set; }
        public decimal PricePerMinute { get; set; }
        public Guid RentalAreaId { get; set; }
        public string RentalAreaName { get; set; }
    }
}