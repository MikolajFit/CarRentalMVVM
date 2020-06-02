using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class LoginViewModel:ViewModelBase
    {
        private ObservableCollection<DriverDTO> _drivers;
        private IDriverService _driverService;
        private DriverDTO _selectedDriver;

        public LoginViewModel(IDriverService driverService)
        {
            Drivers = new ObservableCollection<DriverDTO>(driverService.GetAllDrivers());
            SelectedDriver = Drivers[0];
            SubmitButtonCommand = new RelayCommand(
                NavigateToDriverMainView);
        }

        private void NavigateToDriverMainView()
        {
            Messenger.Default.Send(new NotificationMessage("CloseLoginWindow"));
            Messenger.Default.Send(SelectedDriver);
        }

        public ObservableCollection<DriverDTO> Drivers
        {
            get { return _drivers; }
            set { _drivers = value; }
        }
        public RelayCommand SubmitButtonCommand { get; }

        public DriverDTO SelectedDriver
        {
            get => _selectedDriver;
            set { Set(() => SelectedDriver, ref _selectedDriver, value); }
        }
    }
}
