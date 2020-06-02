using System;
using System.Collections.ObjectModel;
using CarRental.UI.Models;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class RentCarViewModel:ViewModelBase

    {
        private readonly ICarService _carService;
        private readonly IRentalService _rentalService;
        private CarDTO _selectedCar;
        private DriverDTO _currentDriver;

        private DriverDTO CurrentDriver
        {
            get => _currentDriver;
            set { Set(() => CurrentDriver, ref _currentDriver, value); }
        }
        

        public RentCarViewModel(ICarService carService, IRentalService rentalService)
        {
            Messenger.Default.Register<DriverDTO>(this, AssignLoggedInDriver);
            _carService = carService;
            _rentalService = rentalService;
            AvailableCars = new ObservableCollection<CarDTO>(_carService.GetFreeCars());
            RentCarCommand = new RelayCommand(RentSelectedCar,CanExecuteRentCar);
        }

        private bool CanExecuteRentCar() => SelectedCar != null;

        private void AssignLoggedInDriver(DriverDTO driver)
        {
            CurrentDriver = driver;
        }


        private void RentSelectedCar()
        {
            try
            {
                var rentalGuid = Guid.NewGuid();
                _rentalService.TakeCar(rentalGuid,SelectedCar.Id,CurrentDriver.Id,DateTime.Now);
                var rentalInfo = new RentalInfo()
                {
                    RentalId = rentalGuid,
                    SelectedCar = SelectedCar.RegistrationNumber,
                    PricePerMinute = SelectedCar.PricePerMinute
                };
                Messenger.Default.Send(rentalInfo);
                Messenger.Default.Send(new NotificationMessage("Start Car Rental"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public CarDTO SelectedCar
        {
            get => _selectedCar;
            set
            {
                Set(() => SelectedCar, ref _selectedCar, value);
                RentCarCommand.RaiseCanExecuteChanged();
            }
        }


        public ObservableCollection<CarDTO> AvailableCars  { get; set; }

        public RelayCommand RentCarCommand { get; }
     
    }
}