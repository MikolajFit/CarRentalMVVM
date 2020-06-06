using System;
using CarRental.UI.Models;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class RegisterDriverViewModel : CustomViewModelBase
    {
        private readonly IDriverService _driverService;

        public RegisterDriverViewModel(IDriverService driverService)
        {
            _driverService = driverService;
            CurrentDriver = new DriverModel();
            RegisterDriverCommand = new RelayCommand(RegisterDriver,CanRegister);
        }

        private void RegisterDriver()
        {
            _driverService.CreateDriver(new DriverDTO()
            {
                FirstName = CurrentDriver.FirstName,
                Id = Guid.NewGuid(),
                LastName = CurrentDriver.LastName,
                LicenseNumber = CurrentDriver.LicenseNumber
            });
            Messenger.Default.Send(new NotificationMessage("Close Register Window"));
        }

        private DriverModel _currentDriver;

        public DriverModel CurrentDriver
        {
            get => _currentDriver;
            set { Set(() => CurrentDriver, ref _currentDriver, value); }
        }

        public RelayCommand RegisterDriverCommand { get; }
        

        public bool CanRegister()
        {
            return CurrentDriver.IsValid;
        }
    }
}

