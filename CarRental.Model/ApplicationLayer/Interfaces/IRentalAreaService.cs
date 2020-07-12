using System;
using System.Collections.Generic;
using CarRental.Model.ApplicationLayer.DTOs;

namespace CarRental.Model.ApplicationLayer.Interfaces
{
    public interface IRentalAreaService : IApplicationService
    {
        void CreateRentalArea(RentalAreaDTO rentalAreaDto);
        void UpdateRentalArea(RentalAreaDTO rentalAreaDto);
        List<RentalAreaDTO> GetAllRentalAreas();
        RentalAreaDTO GetRentalArea(Guid id);
    }
}