using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class WebView : Page
    {
        public WebView()
        {
            InitializeComponent();
            App.commandBarRefreshVisible = Windows.UI.Xaml.Visibility.Collapsed;
            Windows.Web.Http.HttpRequestMessage requestMsg = new Windows.Web.Http.HttpRequestMessage();
            requestMsg.Headers.Add("Authorization", "Bearer " + App.TLKM.accessToken);
            requestMsg.Method = Windows.Web.Http.HttpMethod.Get;

            if (App.commandBarText == App.resourceLoader.GetString("Eshop/Text").ToUpper())
            {
                wview.Source = new System.Uri("https://www.telekom.sk/eshop-telekom?traffic_source=app_webview");
            }
            else if (App.commandBarText == App.resourceLoader.GetString("Gifts/Text").ToUpper())
            {
                requestMsg.RequestUri = new System.Uri("http://m.telekom.sk/api/app-sutaze-akcie/ver2/?zdroj=oneapp_menu");
                wview.NavigateWithHttpRequestMessage(requestMsg);
            }
            else if (App.commandBarText == App.resourceLoader.GetString("Magenta1/Text").ToUpper())
            {
                requestMsg.RequestUri = new System.Uri("https://m.telekom.sk/api/app-sutaze-akcie/ver2/magenta1/?zdroj=oneapp_menu");
                wview.NavigateWithHttpRequestMessage(requestMsg);
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            wview.Height = App.frameHeight;
        }
    }
}
