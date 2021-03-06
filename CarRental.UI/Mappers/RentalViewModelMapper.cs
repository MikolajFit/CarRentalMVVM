﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.UI.ViewModels.ObservableObjects;

namespace CarRental.UI.Mappers
{
    public class RentalViewModelMapper:IRentalViewModelMapper
    {
        public RentalViewModel Map(RentalDTO rentalDto)
        {
            return new RentalViewModel()
            {
                RentalId = rentalDto.Id,
                DriverName = rentalDto.DriverName,
                PricePerMinute = $"{rentalDto.PricePerMinute:0.00}",
                RegistrationNumber = rentalDto.RegistrationNumber,
                StartDateTime = rentalDto.StartDateTime,
                StopDateTime = rentalDto.StopDateTime,
                Total = rentalDto.Total
            };
        }
    }
}
