using CarRental.UI.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public abstract class CustomViewModelBase : ViewModelBase
    {
        protected CustomViewModelBase()
        {
            Messenger.Default.Register<CleanupMessage>(this, CleanViewModel);
        }

        private void CleanViewModel(CleanupMessage obj)
        {
            base.Cleanup();
        }
    }
}