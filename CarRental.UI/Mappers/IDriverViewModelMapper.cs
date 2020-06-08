using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.DTOs;

namespace CarRental.UI.Mappers
{
    public interface IDriverViewModelMapper
    {
        DriverViewModel Map(DriverDTO driverDto);
        DriverDTO Map(DriverViewModel driver);

    }
}
