using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class Profile : Page
    {
        public Profile()
        {
            InitializeComponent();
            App.commandBarRefreshVisible = Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
