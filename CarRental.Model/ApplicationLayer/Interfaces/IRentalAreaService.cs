using System;
using System.Collections.Generic;
using CarRental.Model.ApplicationLayer.DTOs;
using DDD.Base.ApplicationLayer.Services;

namespace CarRental.Model.ApplicationLayer.Interfaces
{
    public interface IRentalAreaService : IApplicationService
    {
        void CreateRentalArea(RentalAreaDTO rentalAreaDto);
        List<RentalAreaDTO> GetAllRentalAreas();
        RentalAreaDTO GetRentalArea(Guid id);
    }
}