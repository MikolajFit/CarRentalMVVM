using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.UI.ViewModels.ObservableObjects;

namespace CarRental.UI.Mappers
{
    public interface ICarViewModelMapper
    {
        CarViewModel Map(CarDTO carDto);
        CarDTO Map(RentalAreaViewModel selectedRentalArea, CarViewModel carViewModel,
            bool provideCustomPosition = false);
    }
}
