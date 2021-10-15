using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class Overview : Page
    {

        public Overview()
        {
            InitializeComponent();
            App.commandBarRefreshVisible = Windows.UI.Xaml.Visibility.Visible;
            Load_Dashboard();
        }

        private async void Load_Dashboard()
        {
            bool dash_success = await System.Threading.Tasks.Task.Run(() => App.TLKM.Dashboard());
            if (dash_success)
            {
                simName.Text = App.TLKM.productLabel + " - " + App.TLKM.productName;
                simData.Text = App.TLKM.remainingGB + "/" + App.TLKM.maxGB + "GB";
                App.TLKM.Update_LiveTile();
            }
            else
            {
                await App.TLKM.ShowError();
            }
            bool unpaidbills_success = await System.Threading.Tasks.Task.Run(() => App.TLKM.Unpaid_Bills());
            if (unpaidbills_success)
            {
                ubill_count.Text = App.TLKM.unpaidBillsCount + " " + App.resourceLoader.GetString("Invoice");
                ubill_amount.Text = App.TLKM.unpaidBillsAmount + " " + App.TLKM.unpaidBillsCurrency;
            }
            else
            {
                await App.TLKM.ShowError();
            }
            bool prodreport_success = await System.Threading.Tasks.Task.Run(() => App.TLKM.ProductReport());
            if (prodreport_success)
            {
            }
            else
            {
                await App.TLKM.ShowError();
            }
        }
    }
}
