using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.DTOs;

namespace CarRental.UI.Mappers
{
    public interface IRentalViewModelMapper
    {
        RentalViewModel Map(RentalDTO rentalDto);
    }
}