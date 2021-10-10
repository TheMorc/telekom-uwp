using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class Eshop : Page
    {
        public Eshop()
        {
            InitializeComponent();
            App.commandBarRefreshVisible = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            wview.Height = App.frameHeight;
        }
    }
}
