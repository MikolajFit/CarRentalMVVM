using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;


namespace CarRental.UI.Services
{

    public class MessengerService : IMessengerService
    {
        public void ShowMessage(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
