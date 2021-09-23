using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Telekom
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class setup_pin : Page
    {

        public setup_pin()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += goBack;


        }

        private void goBack(object sender, BackRequestedEventArgs e)
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

            Boolean success = await System.Threading.Tasks.Task.Run(() => App.TLKM.pin(parsedNumber));
            if (success)
            {
                Debug.WriteLine("[tlkm_setup_pin] pin sent successfully");

                Frame.Navigate(typeof(setup_verif));
            }
            else
                await App.TLKM.showError();

        }
    }
}
