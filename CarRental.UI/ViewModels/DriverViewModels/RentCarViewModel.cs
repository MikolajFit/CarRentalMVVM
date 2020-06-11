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


        public RentCarViewModel(ICarService carService, IRentalService rentalService,
            ICarViewModelMapper carViewModelMapper, IRentalViewModelMapper rentalViewModelMapper, IMessengerService messengerService)
        {
            _carService = carService;
            _rentalService = rentalService;
            _carViewModelMapper = carViewModelMapper;
            _rentalViewModelMapper = rentalViewModelMapper;
            _messengerService = messengerService;
            PopulateAvailableCarListView();
            RentCarCommand = new RelayCommand(RentCar, CanExecuteRentCar);
        }

        public CarViewModel SelectedCar
        {
            get => _selectedCar;
            set { Set(() => SelectedCar, ref _selectedCar, value); }
        }

        public ObservableCollection<CarViewModel> AvailableCars { get; set; } =
            new ObservableCollection<CarViewModel>();

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
            var rentals = _rentalService.GetRentalsForDriver(CurrentDriver.Id);
            var activeRental = rentals.FirstOrDefault(r => r.StopDateTime.HasValue == false);
            if (activeRental == null) return;
            SendRentalViewModelMessage(activeRental, RentalViewModelMessageType.ContinueRental);
        }

        private void PopulateAvailableCarListView()
        {
            var cars = _carService.GetFreeCars();
            AvailableCars.Clear();
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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