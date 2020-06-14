using System.Windows;

namespace CarRental.UI.Views
{
    /// <summary>
    ///     Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string message, MessageType Type)
        {
            InitializeComponent();
            txtMessage.Text = message;
            switch (Type)
            {
                case MessageType.Info:
                    txtTitle.Text = "Info";
                    break;
                case MessageType.Confirmation:
                    txtTitle.Text = "Confirmation";
                    break;
                case MessageType.Success:
                {
                    txtTitle.Text = "Success";
                }
                    break;
                case MessageType.Warning:
                    txtTitle.Text = "Warning";
                    break;
                case MessageType.Error:
                {
                    txtTitle.Text = "Error";
                    break;
                }
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

    public enum MessageType
    {
        Info,
        Confirmation,
        Success,
        Warning,
        Error,
    }

}