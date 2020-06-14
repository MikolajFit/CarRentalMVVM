using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using CarRental.UI.Views;


namespace CarRental.UI.Services
{

    public class DialogService : IDialogService
    {
        public void ShowMessage(string text)
        {
            new CustomMessageBox(text, MessageType.Confirmation).ShowDialog();
        }
    }
}
