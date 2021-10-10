using Windows.UI.Xaml.Controls;

namespace Telekom.Views
{
    public sealed partial class Profile : Page
    {
        public Profile()
        {
            InitializeComponent();
            App.commandBarRefreshVisible = Windows.UI.Xaml.Visibility.Collapsed;
            Load_Profile();
        }

        private void Load_Profile()
        {
            simLabel.Text = App.TLKM.productLabel;
            firstName.Text = App.TLKM.givenName;
            lastName.Text = App.TLKM.familyName;
            contactTelephoneNumber.Text = "+" + App.TLKM.serviceId.ToString();
            telekom_username.Text = App.TLKM.telekom_username;
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            bool patch_success = await System.Threading.Tasks.Task.Run(() => App.TLKM.PatchProfile(simLabel.Text, firstName.Text, lastName.Text, contactTelephoneNumber.Text));
            if (!patch_success)
            {
                await App.TLKM.ShowError();
            }
        }
    }
}
