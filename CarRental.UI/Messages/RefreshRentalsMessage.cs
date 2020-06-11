using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.Messages
{
    public class RefreshRentalsMessage :NotificationMessage
    {
        public RefreshRentalsMessage(string notification) : base(notification)
        {
        }
    }
}
