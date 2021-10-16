using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class Error : Page
    {

        public Error()
        {
            InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += GoBack;

            if (App.TLKM.lastCode == "nointernet")
            {
                error_icon.Glyph = "";
                error_code.Text = "";
                error_code.Visibility = Visibility.Collapsed;
                error_reason.Text = App.resourceLoader.GetString("error_reason_nointernet/Text");
                error_block.Text = App.resourceLoader.GetString("error_block_nointernet/Text");
            }
            else
            {
                error_icon.Glyph = "";
                error_code.Text = App.TLKM.lastCode;
                error_code.Visibility = Visibility.Visible;
                error_reason.Text = App.resourceLoader.GetString("error_reason_any/Text");
                error_block.Text = App.resourceLoader.GetString("error_block_any/Text");
            }

        }

        private void GoBack(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ExtendedSplash extendedSplash = new ExtendedSplash(App.TLKM.splashScreen, false);
            Window.Current.Content = extendedSplash;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
