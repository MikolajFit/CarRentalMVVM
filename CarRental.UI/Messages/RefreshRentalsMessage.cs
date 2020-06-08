using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.Messages
{
    public class RefreshRentalsMessage :NotificationMessage
    {
        public RefreshRentalsMessage(string notification) : base(notification)
        {
        }

        public RefreshRentalsMessage(object sender, string notification) : base(sender, notification)
        {
        }

        public RefreshRentalsMessage(object sender, object target, string notification) : base(sender, target, notification)
        {
        }
    }
}
