using CarRental.UI.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class CustomViewModelBase : ViewModelBase
    {
        public CustomViewModelBase()
        {
            Messenger.Default.Register<CleanupMessage>(this, CleanViewModel);
        }

        private void CleanViewModel(CleanupMessage obj)
        {
            base.Cleanup();
        }
    }
}