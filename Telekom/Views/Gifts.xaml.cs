using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class Gifts : Page
    {
        public Gifts()
        {
            InitializeComponent();
            App.commandBarRefreshVisible = Windows.UI.Xaml.Visibility.Collapsed;

            Windows.Web.Http.HttpRequestMessage requestMsg = new Windows.Web.Http.HttpRequestMessage(Windows.Web.Http.HttpMethod.Get, new System.Uri("http://m.telekom.sk/api/app-sutaze-akcie/ver2/?zdroj=oneapp_menu"));
            requestMsg.Headers.Add("Authorization", "Bearer " + App.TLKM.accessToken);
            wview.NavigateWithHttpRequestMessage(requestMsg);
        }

        private void Page_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            wview.Height = App.frameHeight;
        }
    }
}
