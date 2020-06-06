using System;
using System.Collections.ObjectModel;
using CarRental.UI.Models;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels.DriverViewModels
{
    public class RentCarViewModel : CustomViewModelBase

    {
        private readonly ICarService _carService;
        private readonly IRentalService _rentalService;
        private DriverDTO _currentDriver;
        private CarDTO _selectedCar;


        public RentCarViewModel(ICarService carService, IRentalService rentalService)
        {
            Messenger.Default.Register<DriverDTO>(this, AssignLoggedInDriver);
            _carService = carService;
            _rentalService = rentalService;
            AvailableCars = new ObservableCollection<CarDTO>(_carService.GetFreeCars());
            RentCarCommand = new RelayCommand(RentSelectedCar, CanExecuteRentCar);
        }

        private DriverDTO CurrentDriver
        {
            get => _currentDriver;
            set { Set(() => CurrentDriver, ref _currentDriver, value); }
        }

        public CarDTO SelectedCar
        {
            get => _selectedCar;
            set
            {
                Set(() => SelectedCar, ref _selectedCar, value);
            }
        }


        public ObservableCollection<CarDTO> AvailableCars { get; set; }

        public RelayCommand RentCarCommand { get; }

        private bool CanExecuteRentCar()
        {
            return SelectedCar != null;
        }

        private void AssignLoggedInDriver(DriverDTO driver)
        {
            CurrentDriver = driver;
        }

        public override void Cleanup()
        {
            ViewModelLocator.Cleanup();
            base.Cleanup();
        }

        private void RentSelectedCar()
        {
            try
            {
                var rentalGuid = Guid.NewGuid();
                _rentalService.TakeCar(rentalGuid, SelectedCar.Id, CurrentDriver.Id, DateTime.Now);
                var rentalInfo = new RentalInfo
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
    }
}