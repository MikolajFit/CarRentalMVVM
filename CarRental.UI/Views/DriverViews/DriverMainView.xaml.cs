using System.Windows;
using CarRental.UI.ViewModels;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.Views.DriverViews
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
