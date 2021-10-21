using System;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class Setup_verif : Page
    {
        public Setup_verif()
        {
            InitializeComponent();
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
            long PIN = long.Parse(serviceId.Text);
            bool success = await System.Threading.Tasks.Task.Run(() => App.TLKM.Verif(PIN));
            if (success)
            {
                Debug.WriteLine("[tlkm_setup_verif] pin verified successfully");
                bool login_success = await System.Threading.Tasks.Task.Run(() => App.TLKM.Login());
                if (login_success)
                {
                    Debug.WriteLine("[tlkm_setup_verif] logged in successfully!");
                    Frame.Navigate(typeof(AppShell));
                }
                else
                {
                    await App.TLKM.ShowMessage();
                }
            }
            else
            {
                await App.TLKM.ShowMessage();
            }
        }

    }
}
