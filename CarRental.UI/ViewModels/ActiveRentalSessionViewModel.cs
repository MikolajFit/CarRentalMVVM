using System;
using CarRental.UI.Models;
using CarRental.UI.Utils.Interfaces;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class ActiveRentalSessionViewModel : ViewModelBase
    {
        private readonly IRentalService _rentalService;
        private readonly ITimer _timer;
        private readonly ITimerFactory _timerFactory;
        private string _carInfo;
        private decimal _pricePerMinute;
        private RentalInfo _rentalInfo;
        private string _timerText;
        private int _totalSeconds;
        private bool _isRentalStopped = false;

        public ActiveRentalSessionViewModel(ITimerFactory timerFactory, IRentalService rentalService)
        {
            Messenger.Default.Register<RentalInfo>(this, StartRental);
            _timerFactory = timerFactory;
            _rentalService = rentalService;
            _timer = timerFactory.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += UpdateTimerState;
            StopRentalCommand = new RelayCommand(StopRental);
            CloseActiveRentalSessionViewCommand = new RelayCommand(CloseActiveRentalSessionView, IsRentalStop);
        }

        private bool IsRentalStop()
        {
            return IsRentalStopped;
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

        public RelayCommand StopRentalCommand { get; }
        public RelayCommand CloseActiveRentalSessionViewCommand { get; }

        private void CloseActiveRentalSessionView()
        {
            Messenger.Default.Send(new NotificationMessage("Stop Car Rental"));
        }

        private void StopRental()
        {
            _rentalService.ReturnCar(_rentalInfo.RentalId, DateTime.Now);
            IsRentalStopped = true;
            CloseActiveRentalSessionViewCommand.RaiseCanExecuteChanged();
            StopTimer();
        }

        private void StartRental(RentalInfo rentalInfo)
        {
            _rentalInfo = rentalInfo;
            CarInfo = _rentalInfo.SelectedCar;
            PricePerMinute = _rentalInfo.PricePerMinute;
            TimerText = $"{TimeSpan.FromSeconds(_totalSeconds).Duration():hh\\:mm\\:ss}";
            _timer.Start();
        }

        private void StopTimer()
        {
            _timer.Stop();
            _totalSeconds = 0;
        }

        private void UpdateTimerState(object sender, EventArgs e)
        {
            _totalSeconds += 1;
            TimerText = $"{TimeSpan.FromSeconds(_totalSeconds).Duration():hh\\:mm\\:ss}";
        }
    }
}