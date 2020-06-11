using CarRental.UI.Services;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels.DriverViewModels
{
    public class DriverMainViewModel : MainViewModel
    {
        private readonly ActiveRentalSessionViewModel _activeRentalSessionViewModel;
        private readonly RentCarViewModel _rentCarViewModel;
        private CustomViewModelBase _currentRentCarViewModel;

        public DriverMainViewModel(IMessengerService messengerService) : base(messengerService)
        {
            Messenger.Default.Register<NotificationMessage>(this, SwitchRentalView);
            _rentCarViewModel = SimpleIoc.Default.GetInstance<RentCarViewModel>();
            _activeRentalSessionViewModel = SimpleIoc.Default.GetInstance<ActiveRentalSessionViewModel>();
            CurrentRentCarViewModel = _rentCarViewModel;
        }


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
                case "Stop Car Rental":
                    CurrentRentCarViewModel = _rentCarViewModel;
                    break;
            }
        }
    }
}