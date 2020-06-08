using System;
using CarRental.UI.Mappers;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class RegisterDriverViewModel : CustomViewModelBase
    {
        private readonly IDriverService _driverService;
        private readonly IDriverViewModelMapper _driverViewModelMapper;

        private DriverViewModel _currentDriver;

        public RegisterDriverViewModel(IDriverService driverService, IDriverViewModelMapper driverViewModelMapper)
        {
            _driverService = driverService;
            _driverViewModelMapper = driverViewModelMapper;
            CurrentDriver = new DriverViewModel
            {
                Id = Guid.NewGuid()
            };
            RegisterDriverCommand = new RelayCommand(RegisterDriver, CanRegister);
        }

        public DriverViewModel CurrentDriver
        {
            get => _currentDriver;
            set { Set(() => CurrentDriver, ref _currentDriver, value); }
        }

        public RelayCommand RegisterDriverCommand { get; }

        private void RegisterDriver()
        {
            var driverDto = _driverViewModelMapper.Map(CurrentDriver);
            _driverService.CreateDriver(driverDto);
            Messenger.Default.Send(new NotificationMessage("Close Register Window"));
        }


        public bool CanRegister()
        {
            return CurrentDriver.IsValid;
        }
    }
}