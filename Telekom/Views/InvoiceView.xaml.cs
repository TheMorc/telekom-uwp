using System.Collections.Generic;
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
                List<moreInformationItems> mII = new List<moreInformationItems>();
                moreInformationItems tempMII = new moreInformationItems();
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

                tempMII = new moreInformationItems
                {
                    Name = "Číslo adresáta",
                    Value = bill.BillingAccount.BusinessId
                };
                mII.Add(tempMII);

                tempMII = new moreInformationItems
                {
                    Name = "Variable symbol",
                    Value = bill.BillingAccount.Name
                };
                mII.Add(tempMII);

                foreach (JSON_.BillingRate bRate in bill.BillingRates)
                {
                    tempMII = new moreInformationItems
                    {
                        Name = bRate.Name,
                        Value = bRate.TaxIncludedAmount.Amount + bRate.TaxIncludedAmount.Units
                    };

                    mII.Add(tempMII);
                }

                tempMII = new moreInformationItems
                {
                    Name = "Suma spolu s DPH",
                    Value = bill.InvoiceAmount.Amount + bill.InvoiceAmount.Units
                };
                mII.Add(tempMII);

                tempMII = new moreInformationItems
                {
                    Name = $"{bill.TaxItem[0].TaxCategory} {bill.TaxItem[0].TaxRate}%",
                    Value = bill.TaxItem[0].TaxAmount.Amount + bill.TaxItem[0].TaxAmount.Units
                };
                mII.Add(tempMII);

                tempMII = new moreInformationItems
                {
                    Name = "Suma faktúry",
                    Value = bill.TaxIncludedAmount.Amount + bill.TaxIncludedAmount.Units
                };
                mII.Add(tempMII);


                moreInformation.ItemsSource = mII;
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
                App.commandBarText = App.resourceLoader.GetString("Invoices/Text").ToUpper();
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

    public class moreInformationItems
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
}
