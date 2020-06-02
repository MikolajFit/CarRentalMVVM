using System.Collections.Generic;
using System.Linq;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.DomainModelLayer.Models;

namespace DDD.CarRentalLib.ApplicationLayer.Mappers
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
                Area = new List<PositionDTO>()
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