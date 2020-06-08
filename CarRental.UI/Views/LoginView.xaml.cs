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
using CarRental.UI.Views.AdminViews;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "GoToDriverMainWindow":
                        ViewModelLocator.Cleanup();
                        var driverMainView = new DriverViews.DriverMainView();
                        driverMainView.Show();
                        Messenger.Default.Unregister<NotificationMessage>(this);
                        this.Close();
                        break;
                    case "GoToRegisterWindow":
                        ViewModelLocator.Cleanup();
                        var registerView = new RegisterDriverView();
                        registerView.Show();
                        Messenger.Default.Unregister<NotificationMessage>(this);
                        this.Close();
                        break;
                    case "GoToAdminMainView":
                        ViewModelLocator.Cleanup();
                        var adminView = new AdminMainView();
                        adminView.Show();
                        Messenger.Default.Unregister<NotificationMessage>(this);
                        this.Close();
                        break;
                }
            });
        }
    }
}
