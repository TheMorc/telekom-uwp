using Windows.UI.Xaml.Controls;

namespace Telekom
{
    public sealed partial class Dashboard : Page
    {

        public Dashboard()
        {
            this.InitializeComponent();
            simName.Text = App.TLKM.productLabel + " - " + App.TLKM.productName;
            simData.Text = App.TLKM.remainingGB + "/" + App.TLKM.maxGB + "GB";
        }

    }
}
