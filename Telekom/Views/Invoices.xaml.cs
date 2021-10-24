using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class Invoices : Page
    {
        public Invoices()
        {
            InitializeComponent();
            App.commandBarRefreshVisible = Windows.UI.Xaml.Visibility.Collapsed;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested += null;

            Load_Months();
        }

        private async void Load_Months()
        {
            List<JSON_.BillingMonths> billMonths = await System.Threading.Tasks.Task.Run(() => App.TLKM.BillingMonths(24));
            if (billMonths != null)
            {
                InvoiceList.ItemsSource = billMonths;
            }
            else
            {
                await App.TLKM.ShowMessage();
            }
        }

        private async void InvoiceList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.BackStack.Clear();
            Frame.BackStack.Add(new Windows.UI.Xaml.Navigation.PageStackEntry(typeof(Invoices), null, null));
            JSON_.BillingMonths item = e.ClickedItem as JSON_.BillingMonths;
            Debug.WriteLine("[tlkm_invoicelist] clicked " + item.Month + "/" + item.Year);
            List<JSON_.CustomerBills> custBills = await System.Threading.Tasks.Task.Run(() => App.TLKM.CustomerBills(item.Month, item.Year));
            if (custBills != null)
            {
                App.TLKM.customerBills = custBills;
                Frame.Navigate(typeof(InvoiceView));
            }
            else
            {
                await App.TLKM.ShowMessage();
            }
        }

        private void Page_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            ScrollViewer.Height = App.frameHeight;
        }
    }
}
