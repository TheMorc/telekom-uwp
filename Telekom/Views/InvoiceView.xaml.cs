using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class InvoiceView : Page
    {

        public InvoiceView()
        {
            InitializeComponent();

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += GoBack;

            App.commandBarText = App.resourceLoader.GetString("Invoice_Details") + App.TLKM.customerBills[0].BillDate.Value.ToString("y").ToUpper();

            Load_Details();
        }

        private async void Load_Details()
        {
            JSON_.BillView bill = await System.Threading.Tasks.Task.Run(() => App.TLKM.BillView(App.TLKM.customerBills[0].Id));
            if (bill != null)
            {
                bill_amount.Text = bill.InvoiceAmount.Amount + " " + bill.InvoiceAmount.Units;
                bill_id.Text = bill.BillNo;
                foreach (JSON_.RelatedParty party in bill.RelatedParty)
                {
                    if (party.Role == "billReceiver")
                    {
                        bill_receiver.Text = party.Name;
                    }
                }
                bill_type.Text = bill.Type;
            }
            else
            {
                await App.TLKM.ShowMessage();
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

        private void Page_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            ScrollViewer.Height = App.frameHeight;
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //download 
        }
    }
}
