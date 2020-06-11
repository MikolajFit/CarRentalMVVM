﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.UI.Services
{
    public interface IMessengerService
    {
        void Send<TMessage>(TMessage parameter);
    }
}
