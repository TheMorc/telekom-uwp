using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class Overview : Page
    {

        public Overview()
        {
            InitializeComponent();
            App.commandBarRefreshVisible = Windows.UI.Xaml.Visibility.Visible;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested += null;

            Load_Dashboard();
        }

        private async void Load_Dashboard()
        {

            simName.Text = App.TLKM.prodRep.Label + " - " + App.TLKM.prodRep.Description;
            simData.Text = App.TLKM.prodRep.ConsumptionGroups[0].Consumptions[0].Remaining.Value + App.TLKM.prodRep.ConsumptionGroups[0].Consumptions[0].Remaining.Unit + "/" + App.TLKM.prodRep.ConsumptionGroups[0].Consumptions[0].Max.Value + App.TLKM.prodRep.ConsumptionGroups[0].Consumptions[0].Max.Unit;

            if (App.TLKM.prodRep.Category == "mobilePrepaid")
            {
                simBalance.Text = App.TLKM.prodRep.CreditBalance.Total.Amount + App.TLKM.prodRep.CreditBalance.Total.CurrencyCode;
            }

            App.TLKM.Update_LiveTile();

            bool unpaidbills_success = await System.Threading.Tasks.Task.Run(() => App.TLKM.Unpaid_Bills());
            if (unpaidbills_success)
            {
                if (App.TLKM.unpaidBills.Count.Value != 0)
                {
                    invoice.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    ubill_count.Text = App.TLKM.unpaidBills.Count.Value + " " + App.resourceLoader.GetString("Invoice");
                    ubill_amount.Text = App.TLKM.unpaidBills.Cost.Amount + " " + App.TLKM.unpaidBills.Cost.CurrencyCode;
                }
            }
            else
            {
                await App.TLKM.ShowMessage();
            }

        }
    }
}
