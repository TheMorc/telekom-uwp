using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
            App.commandBarRefreshVisible = Windows.UI.Xaml.Visibility.Collapsed;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested += null;
        }
    }
}
