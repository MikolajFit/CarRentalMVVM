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
        private readonly IDialogService _dialogService;
        private readonly IMessengerService _messengerService;
        private readonly IRentalService _rentalService;
        private readonly ITimerFactory _timerFactory;
        private RentalViewModel _currentRental;
        private ITimer _elapsedTimer;
        private string _estimatedCost;
        private ITimer _estimatedCostTimer;
        private bool _isRentalStopped;
        private string _timerText;
        private int _totalSeconds;

        public ActiveRentalSessionViewModel(ITimerFactory timerFactory, IRentalService rentalService,
            IDialogService dialogService, IMessengerService messengerService)
        {
            _timerFactory = timerFactory;
            _rentalService = rentalService;
            _dialogService = dialogService;
            _messengerService = messengerService;
            Messenger.Default.Register<RentalViewModelMessage>(this, ConfigureActiveRentalView);
            StopRentalCommand = new RelayCommand(StopRental, () => !IsRentalStopped);
            CloseActiveRentalSessionViewCommand = new RelayCommand(CloseActiveRentalSessionView, () => IsRentalStopped);
            ConfigureTimers();
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

        public RentalViewModel CurrentRental
        {
            get => _currentRental;
            set { Set(() => CurrentRental, ref _currentRental, value); }
        }


        public string EstimatedCost
        {
            get => _estimatedCost;
            set { Set(() => EstimatedCost, ref _estimatedCost, value); }
        }

        public RelayCommand StopRentalCommand { get; }
        public RelayCommand CloseActiveRentalSessionViewCommand { get; }

        private void ConfigureTimers()
        {
            _elapsedTimer = _timerFactory.CreateTimer();
            _elapsedTimer.Interval = TimeSpan.FromSeconds(1);
            _elapsedTimer.Tick += UpdateElapsedTimerState;
            _estimatedCostTimer = _timerFactory.CreateTimer();
            _estimatedCostTimer.Interval = TimeSpan.FromSeconds(60);
            _estimatedCostTimer.Tick += UpdateEstimatedCostTimerState;
        }

        private void ConfigureActiveRentalView(RentalViewModelMessage message)
        {
            _currentRental = message.RentalViewModel;
            switch (message.MessageType)
            {
                case RentalViewModelMessageType.StartRental:
                {
                    _messengerService.Send(new RefreshRentalsMessage("Rental Started!"));
                    break;
                }
                case RentalViewModelMessageType.ContinueRental:
                {
                    _totalSeconds = (int) (DateTime.Now - _currentRental.StartDateTime).TotalSeconds;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UpdateTimerText();
            UpdateEstimatedCostText();
            _elapsedTimer.Start();
            _estimatedCostTimer.Start();
        }


        private void UpdateEstimatedCostTimerState(object sender, EventArgs e)
        {
            UpdateEstimatedCostText();
        }

        private void UpdateEstimatedCostText()
        {
            var totalMinutes = Math.Ceiling((double) _totalSeconds / 60);
            var estimated = (int) totalMinutes * double.Parse(CurrentRental.PricePerMinute);
            EstimatedCost = $"{estimated:0.00}";
        }

        private void CloseActiveRentalSessionView()
        {
            _messengerService.Send(new NotificationMessage("Stop Car Rental"));
            CleanResources();
        }

        private void CleanResources()
        {
            _totalSeconds = 0;
            IsRentalStopped = false;
        }


        private void StopRental()
        {
            _rentalService.ReturnCar(_currentRental.RentalId, DateTime.Now);
            _messengerService.Send(new RefreshRentalsMessage("Rental Stopped!"));
            IsRentalStopped = true;
            StopTimer();
            ShowSummary();
        }

        private void ShowSummary()
        {
            var rental = _rentalService.GetRental(_currentRental.RentalId);
            _dialogService.ShowMessage(
                $"Rental Finished! It took {TimerText} and total cost is {rental.Total.ToString("C", CultureInfo.CurrentCulture)}",
                "Rental Summary");
        }

        private void StopTimer()
        {
            _estimatedCostTimer.Stop();
            _elapsedTimer.Stop();
        }

        private void UpdateElapsedTimerState(object sender, EventArgs e)
        {
            _totalSeconds += 1;
            UpdateTimerText();
        }

        private void UpdateTimerText()
        {
            var time = TimeSpan.FromSeconds(_totalSeconds).Duration();
            if (time.Days >= 1)
            {
                TimerText = $"{time:dd} days {time:hh\\:mm\\:ss}";
            }
            else
            {
                TimerText = $"{TimeSpan.FromSeconds(_totalSeconds).Duration():hh\\:mm\\:ss}";
            }
        }
    }
}