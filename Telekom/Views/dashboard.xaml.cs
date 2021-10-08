using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class Dashboard : Page
    {

        public Dashboard()
        {
            InitializeComponent();
            simName.Text = App.TLKM.productLabel + " - " + App.TLKM.productName;
            simData.Text = App.TLKM.remainingGB + "/" + App.TLKM.maxGB + "GB";
            App.TLKM.Update_LiveTile();
        }

        private async void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            bool success = await System.Threading.Tasks.Task.Run(() => App.TLKM.Dashboard());
            if (success)
            {
                simName.Text = App.TLKM.productLabel + " - " + App.TLKM.productName;
                simData.Text = App.TLKM.remainingGB + "/" + App.TLKM.maxGB + "GB";
            }
            else
            {
                await App.TLKM.ShowError();
            }
        }
    }
}
