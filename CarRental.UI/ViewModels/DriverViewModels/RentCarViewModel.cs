using System;
using System.Collections.ObjectModel;
using System.Linq;
using CarRental.UI.Mappers;
using CarRental.UI.Messages;
using CarRental.UI.Services;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using DDD.CarRentalLib.DomainModelLayer.Models;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels.DriverViewModels
{
    public class RentCarViewModel : AssignedDriverViewModelBase
    {
        private const string DriverBanned = "BANNED";
        private readonly ICarService _carService;
        private readonly ICarViewModelMapper _carViewModelMapper;
        private readonly IRentalService _rentalService;
        private readonly IRentalViewModelMapper _rentalViewModelMapper;
        private readonly IMessengerService _messengerService;
        private string _driverBannedError;
        private bool _isCarListEnabled;
        private CarViewModel _selectedCar;
        private string _errorString;


        public RentCarViewModel(ICarService carService, IRentalService rentalService,
            ICarViewModelMapper carViewModelMapper, IRentalViewModelMapper rentalViewModelMapper, IMessengerService messengerService)
        {
            AvailableCars = new ObservableCollection<CarViewModel>();
            _carService = carService ?? throw new ArgumentNullException();
            _rentalService = rentalService ?? throw new ArgumentNullException();
            _carViewModelMapper = carViewModelMapper ?? throw new ArgumentNullException();
            _rentalViewModelMapper = rentalViewModelMapper ?? throw new ArgumentNullException();
            _messengerService = messengerService ?? throw new ArgumentNullException();
            PopulateAvailableCarListView();
            RentCarCommand = new RelayCommand(RentCar, CanExecuteRentCar);
        }

        public CarViewModel SelectedCar
        {
            get => _selectedCar;
            set { Set(() => SelectedCar, ref _selectedCar, value); }
        }

        public ObservableCollection<CarViewModel> AvailableCars { get; set; } 

        public RelayCommand RentCarCommand { get; }

        public string DriverBannedError
        {
            get => _driverBannedError;
            set { Set(() => DriverBannedError, ref _driverBannedError, value); }
        }

        public bool IsCarListEnabled
        {
            get => _isCarListEnabled;
            set { Set(() => IsCarListEnabled, ref _isCarListEnabled, value); }
        }

        public override void AssignLoggedInDriver(DriverViewModel driverViewModel)
        {
            base.AssignLoggedInDriver(driverViewModel);
            CheckIfDriverIsBanned();
            CheckIfRentalIsActive();
        }

        private void CheckIfDriverIsBanned()
        {
            var isDriverBanned = CurrentDriver.DriverStatus == DriverStatus.Banned;
            if (isDriverBanned)
            {
                DriverBannedError = DriverBanned;
                IsCarListEnabled = false;
            }
            else
            {
                IsCarListEnabled = true;
                DriverBannedError = null;
            }
        }

        private void CheckIfRentalIsActive()
        {
            var rental = _rentalService.GetActiveRentalForDriver(CurrentDriver.Id);
            if (rental == null) return;
            SendRentalViewModelMessage(rental, RentalViewModelMessageType.ContinueRental);
        }

        private void PopulateAvailableCarListView()
        {
            AvailableCars.Clear();
            var cars = _carService.GetFreeCars();
            if (cars == null) return;
            foreach (var carViewModel in cars.Select(car => _carViewModelMapper.Map(car)))
                AvailableCars.Add(carViewModel);
        }

        private bool CanExecuteRentCar()
        {
            return SelectedCar != null && CurrentDriver.DriverStatus != DriverStatus.Banned;
        }

        private void RentCar()
        {
            try
            {
                var rentalGuid = Guid.NewGuid();
                _rentalService.TakeCar(rentalGuid, SelectedCar.Id, CurrentDriver.Id, DateTime.Now);
                var rental = _rentalService.GetRental(rentalGuid);
                SendRentalViewModelMessage(rental, RentalViewModelMessageType.StartRental);
                ErrorString = null;
            }
            catch (Exception)
            {
                ErrorString = "Could not rent car.";
            }
        }

        public string ErrorString
        {
            get => _errorString;
            set { Set(() => ErrorString, ref _errorString, value); }
        }

        private void SendRentalViewModelMessage(RentalDTO rental, RentalViewModelMessageType messageType)
        {
            var rentalInfo = _rentalViewModelMapper.Map(rental);
            var message = new RentalViewModelMessage(messageType, rentalInfo);
            _messengerService.Send(message);
            _messengerService.Send(new NotificationMessage("Start Car Rental"));
        }
    }
}