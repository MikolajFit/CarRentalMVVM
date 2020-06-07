using System.Collections.ObjectModel;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class LoginViewModel : CustomViewModelBase
    {
        private DriverDTO _selectedDriver;

        public LoginViewModel(IDriverService driverService)
        {
            Drivers = new ObservableCollection<DriverDTO>(driverService.GetAllDrivers());
            SelectedDriver = Drivers[0];
            LoginCommand = new RelayCommand(
                NavigateToDriverMainView);
            RegisterCommand = new RelayCommand(NavigateToRegisterView);
            AdminLoginCommand = new RelayCommand(NavigateToAdminMainView);
        }


        public ObservableCollection<DriverDTO> Drivers { get; set; }

        public RelayCommand LoginCommand { get; }
        public RelayCommand RegisterCommand { get; }
        public RelayCommand AdminLoginCommand { get; }

        public DriverDTO SelectedDriver
        {
            get => _selectedDriver;
            set { Set(() => SelectedDriver, ref _selectedDriver, value); }
        }

        private void NavigateToAdminMainView()
        {
            Messenger.Default.Send(new NotificationMessage("GoToAdminMainView"));
        }

        private void NavigateToRegisterView()
        {
            Messenger.Default.Send(new NotificationMessage("GoToRegisterWindow"));
        }

        private void NavigateToDriverMainView()
        {
            Messenger.Default.Send(new NotificationMessage("GoToDriverMainWindow"));
            Messenger.Default.Send(SelectedDriver);
        }
    }
}