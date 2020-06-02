using System;
using System.Collections.Generic;
using System.Text;
using CarRental.UI.Utils.Interfaces;

namespace CarRental.UI.Utils
{
    public class TimerFactory : ITimerFactory
    {
        public ITimer CreateTimer() => new Timer();
    }
}
