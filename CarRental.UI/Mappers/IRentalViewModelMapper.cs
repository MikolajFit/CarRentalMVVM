using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.UI.ViewModels.ObservableObjects;

namespace CarRental.UI.Mappers
{
    public interface IRentalViewModelMapper
    {
        RentalViewModel Map(RentalDTO rentalDto);
    }
}