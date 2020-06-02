using System;
using System.Windows.Threading;
using CarRental.UI.Utils.Interfaces;


namespace CarRental.UI.Utils
{
    public class Timer : ITimer
    {
        private readonly DispatcherTimer _internalTimer;

        public Timer()
        {
            _internalTimer = new DispatcherTimer();
        }

        public bool IsEnabled
        {
            get => _internalTimer.IsEnabled;
            set => _internalTimer.IsEnabled = value;
        }

        public TimeSpan Interval
        {
            get => _internalTimer.Interval;
            set => _internalTimer.Interval = value;
        }

        public void Start() => _internalTimer.Start();
        public void Stop() => _internalTimer.Stop();

        public event EventHandler Tick
        {
            add => _internalTimer.Tick += value;
            remove => _internalTimer.Tick -= value;
        }
    }
}
