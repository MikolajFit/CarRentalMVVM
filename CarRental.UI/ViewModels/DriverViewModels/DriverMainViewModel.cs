using System;
using System.Linq;
using CarRental.UI.Mappers;
using CarRental.UI.Messages;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels.DriverViewModels
{
    public class DriverMainViewModel : AssignedDriverViewModelBase
    {
        private readonly IRentalService _rentalService;
        private readonly IRentalViewModelMapper _rentalViewModelMapper;
        private readonly ActiveRentalSessionViewModel _activeRentalSessionViewModel;
        private readonly RentCarViewModel _rentCarViewModel;
        private CustomViewModelBase _currentRentCarViewModel;

        public DriverMainViewModel(IRentalService rentalService, IRentalViewModelMapper rentalViewModelMapper)
        {
            _rentalService = rentalService;
            _rentalViewModelMapper = rentalViewModelMapper;
            Messenger.Default.Register<NotificationMessage>(this, SwitchRentalView);
            _rentCarViewModel = SimpleIoc.Default.GetInstance<RentCarViewModel>();
            _activeRentalSessionViewModel = SimpleIoc.Default.GetInstance<ActiveRentalSessionViewModel>();
            CurrentRentCarViewModel = _rentCarViewModel;
        }

        public override void AssignLoggedInDriver(DriverViewModel driverViewModel)
        {
            base.AssignLoggedInDriver(driverViewModel);
            CheckIfRentalIsActive();
        }
        private void CheckIfRentalIsActive()
        {
            var rentals = _rentalService.GetRentalsForDriver(CurrentDriver.Id);
            var activeRental = rentals.FirstOrDefault(r => r.StopDateTime.HasValue == false);
            if (activeRental == null) return;
            var rentalViewModel = _rentalViewModelMapper.Map(activeRental);
            ContinueRental(rentalViewModel);
        }

        private void ContinueRental(RentalViewModel rental)
        {
            CurrentRentCarViewModel = _activeRentalSessionViewModel;
            var rentalViewModelMessage = new RentalViewModelMessage(RentalViewModelMessageType.ContinueRental, rental);
            Messenger.Default.Send(rentalViewModelMessage);
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