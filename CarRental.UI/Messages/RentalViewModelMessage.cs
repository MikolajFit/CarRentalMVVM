using CarRental.UI.ViewModels.DriverViewModels;
using CarRental.UI.ViewModels.ObservableObjects;

namespace CarRental.UI.Messages
{
    public class RentalViewModelMessage
    {
        public RentalViewModelMessage(RentalViewModelMessageType messageType, RentalViewModel rentalViewModel)
        {
            MessageType = messageType;
            RentalViewModel = rentalViewModel;
        }

        public RentalViewModelMessageType MessageType { get; }
        public RentalViewModel RentalViewModel { get; }
    }
}