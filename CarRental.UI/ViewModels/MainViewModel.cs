using System;
using CarRental.UI.Services;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class MainViewModel : CustomViewModelBase
    {
        private readonly IMessengerService _messengerService;

        public MainViewModel(IMessengerService messengerService)
        {
            _messengerService = messengerService ?? throw new ArgumentNullException();
            LogoutCommand = new RelayCommand(Logout);
        }

        public RelayCommand LogoutCommand { get; }

        private void Logout()
        {
            _messengerService.Send(new NotificationMessage("Logout"));
        }
    }
}