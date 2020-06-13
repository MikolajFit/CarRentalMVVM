using System;
using System.Collections.Generic;

namespace DDD.CarRentalLib.ApplicationLayer.DTOs
{
    public class RentalAreaDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<PositionDTO> Area { get; set; }
        public decimal OutOfBondsPenaltyPerDistanceUnit { get; set; }
        public PositionDTO CarStartingPositionDTO { get; set; }
    }
}