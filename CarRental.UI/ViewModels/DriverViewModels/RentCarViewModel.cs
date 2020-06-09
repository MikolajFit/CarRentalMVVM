using System;
using System.Collections.ObjectModel;
using System.Linq;
using CarRental.UI.Mappers;
using CarRental.UI.Messages;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels.DriverViewModels
{
    public class RentCarViewModel : AssignedDriverViewModelBase

    {
        private readonly ICarService _carService;
        private readonly ICarViewModelMapper _carViewModelMapper;
        private readonly IRentalViewModelMapper _rentalViewModelMapper;
        private readonly IRentalService _rentalService;
        private CarViewModel _selectedCar;


        public RentCarViewModel(ICarService carService, IRentalService rentalService,
            ICarViewModelMapper carViewModelMapper, IRentalViewModelMapper rentalViewModelMapper)
        {
            _carService = carService;
            _rentalService = rentalService;
            _carViewModelMapper = carViewModelMapper;
            _rentalViewModelMapper = rentalViewModelMapper;
            PopulateAvailableCarListView();
            RentCarCommand = new RelayCommand(RentSelectedCar, CanExecuteRentCar);
        }

        public CarViewModel SelectedCar
        {
            get => _selectedCar;
            set { Set(() => SelectedCar, ref _selectedCar, value); }
        }

        public ObservableCollection<CarViewModel> AvailableCars { get; set; } =
            new ObservableCollection<CarViewModel>();

        public RelayCommand RentCarCommand { get; }

        private void PopulateAvailableCarListView()
        {
            var cars = _carService.GetFreeCars();
            AvailableCars.Clear();
            foreach (var carViewModel in cars.Select(car => _carViewModelMapper.Map(car)))
                AvailableCars.Add(carViewModel);
        }

        private bool CanExecuteRentCar()
        {
            return SelectedCar != null;
        }

        private void RentSelectedCar()
        {
            try
            {
                var rentalGuid = Guid.NewGuid();
                _rentalService.TakeCar(rentalGuid, SelectedCar.Id, CurrentDriver.Id, DateTime.Now);
                var rental = _rentalService.GetRental(rentalGuid);
                var rentalInfo = _rentalViewModelMapper.Map(rental);
                var message = new RentalViewModelMessage(RentalViewModelMessageType.StartRental,rentalInfo);
                Messenger.Default.Send(message);
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