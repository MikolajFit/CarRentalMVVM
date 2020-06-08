using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.Messages
{
    class CleanupMessage :NotificationMessage
    {
        public CleanupMessage(string notification) : base(notification)
        {
        }

        public CleanupMessage(object sender, string notification) : base(sender, notification)
        {
        }

        public CleanupMessage(object sender, object target, string notification) : base(sender, target, notification)
        {
        }
    }
}
