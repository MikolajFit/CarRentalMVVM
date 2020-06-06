using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CarRental.UI.ViewModels;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.Views
{
    /// <summary>
    /// Interaction logic for RegisterDriverView.xaml
    /// </summary>
    public partial class RegisterDriverView : Window
    {
        public RegisterDriverView()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "Close Register Window":
                        ViewModelLocator.Cleanup(); 
                        var otherWindow = new LoginView();
                        otherWindow.Show();
                        Messenger.Default.Unregister<NotificationMessage>(this);
                        this.Close();
                        break;
                }
            });
        }
    }
}
