using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.Messages
{
    public class CleanupMessage :NotificationMessage
    {
        public CleanupMessage(string notification) : base(notification)
        {
        }
    }
}
