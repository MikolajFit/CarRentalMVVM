using System.Windows;
using CarRental.UI.ViewModels;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.Views.AdminViews
{
    /// <summary>
    /// Interaction logic for AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainView : Window
    {
        public AdminMainView()
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
