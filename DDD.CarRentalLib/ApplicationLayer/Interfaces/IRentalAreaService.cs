using System;
using System.Collections.Generic;
using DDD.Base.ApplicationLayer.Services;
using DDD.CarRentalLib.ApplicationLayer.DTOs;

namespace DDD.CarRentalLib.ApplicationLayer.Interfaces
{
    public interface IRentalAreaService : IApplicationService
    {
        void CreateRentalArea(RentalAreaDTO rentalAreaDto);
        List<RentalAreaDTO> GetAllRentalAreas();
        RentalAreaDTO GetRentalArea(Guid id);
    }
}