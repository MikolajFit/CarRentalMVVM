using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class DriverMainViewModel:ViewModelBase
    {
        private DriverDTO _currentDriver;
        private ViewModelBase _currentRentCarViewModel;
        private readonly RentCarViewModel _rentCarViewModel;
        private readonly ActiveRentalSessionViewModel _activeRentalSessionViewModel;

        public DriverMainViewModel()
        {
            Messenger.Default.Register<DriverDTO>(this, AssignLoggedInDriver);
            Messenger.Default.Register<NotificationMessage>(this, SwitchRentalView);
            _rentCarViewModel = SimpleIoc.Default.GetInstance<RentCarViewModel>();
            _activeRentalSessionViewModel = SimpleIoc.Default.GetInstance<ActiveRentalSessionViewModel>();
            CurrentRentCarViewModel = _rentCarViewModel;
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

        private void AssignLoggedInDriver(DriverDTO driver)
        {
            CurrentDriver = driver;
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Driver logged in!"));
        }

        public DriverDTO CurrentDriver
        {
            get => _currentDriver;
            set { Set(() => CurrentDriver, ref _currentDriver, value); }
        }

        public ViewModelBase CurrentRentCarViewModel
        {
            get => _currentRentCarViewModel;
            set { Set(() => CurrentRentCarViewModel, ref _currentRentCarViewModel, value); }
        }

    }
}
