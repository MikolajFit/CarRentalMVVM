using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.Services
{
    public class MessengerService:IMessengerService
    {
        public void Send<TMessage>(TMessage parameter)
        {
            Messenger.Default.Send<TMessage>(parameter);
        }
    }
}
