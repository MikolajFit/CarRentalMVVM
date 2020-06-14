using System.Collections.Generic;
using System.Linq;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.DomainModelLayer.Models;

namespace CarRental.Model.ApplicationLayer.Mappers
{
    public class RentalAreaMapper
    {
        public List<RentalAreaDTO> Map(IEnumerable<RentalArea> rentalAreas)
        {
            return rentalAreas.Select(c => Map(c)).ToList();
        }

        public RentalAreaDTO Map(RentalArea r)
        {
            var rental = new RentalAreaDTO
            {
                Id = r.Id,
                Name = r.Name,
                OutOfBondsPenaltyPerDistanceUnit = r.OutOfBondsPenaltyPerDistanceUnit.Amount,
                Area = new List<PositionDTO>(),
                CarStartingPositionDTO = new PositionDTO
                {
                    Latitude = r.CarStartingPosition.Latitude,
                    Longitude = r.CarStartingPosition.Longitude
                }
            };
            foreach (var point in r.Area.Polygon)
                rental.Area.Add(new PositionDTO
                {
                    Latitude = point.Latitude,
                    Longitude = point.Longitude
                });
            return rental;
        }
    }
}