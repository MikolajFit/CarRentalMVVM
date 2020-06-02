using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class DriverRentalsViewModel:ViewModelBase
    {
        private DriverDTO _currentDriver;

        public DriverRentalsViewModel(IRentalService rentalService)
        {
            _rentalService = rentalService;
            Messenger.Default.Register<DriverDTO>(this, AssignLoggedInDriver);
        }

        private void AssignLoggedInDriver(DriverDTO driver)
        {
            CurrentDriver = driver;
            var rentals = _rentalService.GetRentalsForDriver(CurrentDriver.Id);
            DriverRentals = new ObservableCollection<RentalDTO>(rentals);
        }

        private readonly IRentalService _rentalService;

        public DriverDTO CurrentDriver
        {
            get => _currentDriver;
            set { Set(() => CurrentDriver, ref _currentDriver, value); }
        }


        public ObservableCollection<RentalDTO> DriverRentals { get; set; }
    }
}
