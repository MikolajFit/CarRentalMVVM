using System;
using CarRental.UI.Services;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels.DriverViewModels
{
    public class DriverMainViewModel : MainViewModel
    {
        private readonly ActiveRentalSessionViewModel _activeRentalSessionViewModel;
        private readonly RentCarViewModel _rentCarViewModel;
        private CustomViewModelBase _currentRentCarViewModel;

        public DriverMainViewModel(IMessengerService messengerService, RentCarViewModel rentCarViewModel,
            ActiveRentalSessionViewModel activeRentalSessionViewModel, DriverAccountViewModel driverAccountViewModel,
            DriverRentalsViewModel driverRentalsViewModel) : base(messengerService)
        {
            Messenger.Default.Register<NotificationMessage>(this, SwitchRentalView);
            _rentCarViewModel = rentCarViewModel ?? throw new ArgumentNullException();
            _activeRentalSessionViewModel = activeRentalSessionViewModel ?? throw new ArgumentNullException();
            DriverAccountViewModel = driverAccountViewModel ?? throw new ArgumentNullException();
            DriverRentalsViewModel = driverRentalsViewModel ?? throw new ArgumentNullException();
            CurrentRentCarViewModel = _rentCarViewModel;
        }

        public DriverAccountViewModel DriverAccountViewModel { get; }
        public DriverRentalsViewModel DriverRentalsViewModel { get; }
        public CustomViewModelBase CurrentRentCarViewModel
        {
            get => _currentRentCarViewModel;
            set { Set(() => CurrentRentCarViewModel, ref _currentRentCarViewModel, value); }
        }


        private void SwitchRentalView(NotificationMessage message)
        {
            switch (message.Notification)
            {
                case "Start Car Rental":
                    CurrentRentCarViewModel = _activeRentalSessionViewModel;
                    break;
                case "Stop Car Rental" when CurrentRentCarViewModel != _rentCarViewModel:
                    CurrentRentCarViewModel = _rentCarViewModel;
                    break;
            }
        }
    }
}