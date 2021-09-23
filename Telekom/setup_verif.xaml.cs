using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

        private telekom TLKM = new telekom();

        public setup_verif()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            long PIN = long.Parse(serviceId.Text);
            bool success = await System.Threading.Tasks.Task.Run(() => App.TLKM.verif(PIN));
            if (success)
            {
                Debug.WriteLine("[tlkm_setup_verif] pin verified successfully");
            }
            else
                await App.TLKM.showError();
        }
    }
}
