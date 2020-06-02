using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.UI.Utils.Interfaces
{
    public interface ITimer
    {
        TimeSpan Interval { get; set; }
        bool IsEnabled { get; set; }
        void Start();
        void Stop();
        event EventHandler Tick;
    }
}
