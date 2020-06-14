using System;
using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.UI.Mappers;
using CarRental.UI.Services;
using CarRental.UI.ViewModels.ObservableObjects;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class RegisterDriverViewModel : CustomViewModelBase
    {
        private readonly IDriverService _driverService;
        private readonly IDriverViewModelMapper _driverViewModelMapper;
        private readonly IMessengerService _messengerService;

        private DriverViewModel _currentDriver;

        public RegisterDriverViewModel(IDriverService driverService, IDriverViewModelMapper driverViewModelMapper, IMessengerService messengerService)
        {
            _driverService = driverService ?? throw new ArgumentNullException(); ;
            _driverViewModelMapper = driverViewModelMapper ?? throw new ArgumentNullException(); ;
            _messengerService = messengerService ?? throw new ArgumentNullException(); ;
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
            _messengerService.Send(new NotificationMessage("Close Register Window"));
        }


        public bool CanRegister()
        {
            return CurrentDriver.IsValid;
        }
    }
}