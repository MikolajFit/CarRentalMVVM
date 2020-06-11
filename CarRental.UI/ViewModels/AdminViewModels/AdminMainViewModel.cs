using CarRental.UI.Services;

namespace CarRental.UI.ViewModels.AdminViewModels
{
    public class AdminMainViewModel : MainViewModel
    {
        public AdminMainViewModel(IMessengerService messengerService) : base(messengerService)
        {
        }
    }
}