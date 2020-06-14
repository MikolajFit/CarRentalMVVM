using System;
using System.Collections.Generic;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.DomainModelLayer.Models;

namespace CarRental.Model.DomainModelLayer.Factories
{
    public class RentalAreaFactory
    {


        public RentalArea Create(Guid id, decimal outOfBondsPenalty, List<PositionDTO> points, string name)
        {
            return new RentalArea(id, outOfBondsPenalty, points, name);
        }
    }
}