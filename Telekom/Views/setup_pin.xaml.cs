using System;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Telekom
{
    public sealed partial class Setup_pin : Page
    {

        public Setup_pin()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += GoBack;

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
            long parsedNumber = 0;
            if (serviceId.Text.StartsWith("0"))
            {
                parsedNumber = long.Parse("421" + serviceId.Text.Remove(0, 1));
            }
            else if (serviceId.Text.StartsWith("+421"))
            {
                parsedNumber = long.Parse(serviceId.Text.Remove(0, 1));
            }

            Debug.WriteLine("[tlkm_setup_pin] parsed number " + parsedNumber);

            bool success = await System.Threading.Tasks.Task.Run(() => App.TLKM.Pin(parsedNumber));
            if (success)
            {
                Debug.WriteLine("[tlkm_setup_pin] pin sent successfully");

                Frame.Navigate(typeof(Setup_verif));
            }
            else
                await App.TLKM.ShowError();

        }
    }
}
