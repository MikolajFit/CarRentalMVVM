using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CarRental.UI.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DriverMainView : Window
    {
        public DriverMainView()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "Logout":
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
