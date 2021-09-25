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
    public sealed partial class setup_verif : Page
    {
        public setup_verif()
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
            long PIN = long.Parse(serviceId.Text);
            bool success = await System.Threading.Tasks.Task.Run(() => App.TLKM.Verif(PIN));
            if (success)
            {
                Debug.WriteLine("[tlkm_setup_verif] pin verified successfully");
                bool login_success = await System.Threading.Tasks.Task.Run(() => App.TLKM.Login());
                if (login_success)
                {
                    Debug.WriteLine("[tlkm_setup_verif] logged in successfully!");
                    bool dash_success = await System.Threading.Tasks.Task.Run(() => App.TLKM.Dashboard());
                    if (!dash_success)
                    {
                        await App.TLKM.ShowError();
                    }
                    else
                    {
                        Frame.Navigate(typeof(dashboard));
                    }
                }
                else
                    await App.TLKM.ShowError();
            }
            else
                await App.TLKM.ShowError();
        }

    }
}
