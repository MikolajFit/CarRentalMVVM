using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class MainViewModel : CustomViewModelBase
    {
        public MainViewModel()
        {
            LogoutCommand = new RelayCommand(Logout);
        }

        public RelayCommand LogoutCommand { get; }

        private void Logout()
        {
            Messenger.Default.Send(new NotificationMessage("Logout"));
        }
    }
}