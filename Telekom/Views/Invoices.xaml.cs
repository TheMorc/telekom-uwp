using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class Invoices : Page
    {
        public Invoices()
        {
            InitializeComponent();
            App.commandBarRefreshVisible = Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
