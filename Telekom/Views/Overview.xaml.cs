using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class Overview : Page
    {

        public Overview()
        {
            InitializeComponent();
            App.commandBarRefreshVisible = Windows.UI.Xaml.Visibility.Visible;
            simName.Text = App.TLKM.productLabel + " - " + App.TLKM.productName;
            simData.Text = App.TLKM.remainingGB + "/" + App.TLKM.maxGB + "GB";
            App.TLKM.Update_LiveTile();
        }


        /* bool success = await System.Threading.Tasks.Task.Run(() => App.TLKM.Dashboard());
         if (success)
         {
             simName.Text = App.TLKM.productLabel + " - " + App.TLKM.productName;
             simData.Text = App.TLKM.remainingGB + "/" + App.TLKM.maxGB + "GB";
         }
         else
         {
             await App.TLKM.ShowError();
         }*/

    }
}
