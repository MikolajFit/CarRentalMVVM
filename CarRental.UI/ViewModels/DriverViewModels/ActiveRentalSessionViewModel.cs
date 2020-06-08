using System;
using System.Globalization;
using CarRental.UI.Messages;
using CarRental.UI.Services;
using CarRental.UI.Utils.Interfaces;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels.DriverViewModels
{
    public class ActiveRentalSessionViewModel : CustomViewModelBase
    {
        private readonly ITimer _elapsedTimer;
        private readonly ITimer _estimatedCostTimer;
        private readonly IMessengerService _messengerService;
        private readonly IRentalService _rentalService;
        private string _carInfo;
        private string _estimatedCost;
        private bool _isRentalStopped;
        private decimal _pricePerMinute;
        private RentalInfoViewModel _rentalInfo;
        private string _timerText;
        private int _totalSeconds;

        public ActiveRentalSessionViewModel(ITimerFactory timerFactory, IRentalService rentalService,
            IMessengerService messengerService)
        {
            Messenger.Default.Register<RentalInfoViewModel>(this, StartRental);
            _rentalService = rentalService;
            _messengerService = messengerService;
            _elapsedTimer = timerFactory.CreateTimer();
            _elapsedTimer.Interval = TimeSpan.FromSeconds(1);
            _elapsedTimer.Tick += UpdateElapsedTimerState;
            _estimatedCostTimer = timerFactory.CreateTimer();
            _estimatedCostTimer.Interval = TimeSpan.FromSeconds(60);
            _estimatedCostTimer.Tick += UpdateEstimatedCostTimerState;
            StopRentalCommand = new RelayCommand(StopRental, () => !IsRentalStopped);
            CloseActiveRentalSessionViewCommand = new RelayCommand(CloseActiveRentalSessionView, () => IsRentalStopped);
        }

        public bool IsRentalStopped
        {
            get => _isRentalStopped;
            set { Set(() => IsRentalStopped, ref _isRentalStopped, value); }
        }

        public string TimerText
        {
            get => _timerText;
            set { Set(() => TimerText, ref _timerText, value); }
        }

        public string CarInfo
        {
            get => _carInfo;
            set { Set(() => CarInfo, ref _carInfo, value); }
        }

        public decimal PricePerMinute
        {
            get => _pricePerMinute;
            set { Set(() => PricePerMinute, ref _pricePerMinute, value); }
        }

        public string EstimatedCost
        {
            get => _estimatedCost;
            set { Set(() => EstimatedCost, ref _estimatedCost, value); }
        }

        public RelayCommand StopRentalCommand { get; }
        public RelayCommand CloseActiveRentalSessionViewCommand { get; }


        private void UpdateEstimatedCostTimerState(object sender, EventArgs e)
        {
            var totalMinutes = Math.Ceiling((double) _totalSeconds / 60);
            var estimated = (int) totalMinutes * PricePerMinute;
            EstimatedCost = $"{estimated}";
        }

        private void CloseActiveRentalSessionView()
        {
            Messenger.Default.Send(new NotificationMessage("Stop Car Rental"));
            CleanResources();
        }

        private void CleanResources()
        {
            _totalSeconds = 0;
            IsRentalStopped = false;
        }


        private void StopRental()
        {
            _rentalService.ReturnCar(_rentalInfo.RentalId, DateTime.Now);
            Messenger.Default.Send(new RefreshRentalsMessage("Rental Stopped!"));
            IsRentalStopped = true;
            StopTimer();
            ShowSummary();
        }

        private void ShowSummary()
        {
            var rental = _rentalService.GetRental(_rentalInfo.RentalId);
            _messengerService.ShowMessage(
                $"Rental Finished! It took {TimerText} and total cost is {rental.Total.ToString("C", CultureInfo.CurrentCulture)}",
                "Rental Summary");
        }

        private void StartRental(RentalInfoViewModel rentalInfo)
        {
            Messenger.Default.Send(new RefreshRentalsMessage("Rental Started!"));
            _rentalInfo = rentalInfo;
            CarInfo = _rentalInfo.SelectedCar;
            PricePerMinute = _rentalInfo.PricePerMinute;
            TimerText = $"{TimeSpan.FromSeconds(_totalSeconds).Duration():hh\\:mm\\:ss}";
            _elapsedTimer.Start();
            _estimatedCostTimer.Start();
        }

        private void StopTimer()
        {
            _estimatedCostTimer.Stop();
            _elapsedTimer.Stop();
        }

        private void UpdateElapsedTimerState(object sender, EventArgs e)
        {
            _totalSeconds += 1;
            TimerText = $"{TimeSpan.FromSeconds(_totalSeconds).Duration():hh\\:mm\\:ss}";
        }
    }
}