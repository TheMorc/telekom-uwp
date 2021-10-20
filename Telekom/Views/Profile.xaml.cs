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
            simLabel.Text = App.TLKM.login.ManageableAssets[0].Label;
            firstName.Text = App.TLKM.login.Individual.GivenName;
            lastName.Text = App.TLKM.login.Individual.FamilyName;
            contactTelephoneNumber.Text = "+" + App.TLKM.serviceId.ToString();
            telekom_username.Text = App.TLKM.login.Characteristics[0].Value;
            emailAddress.Text = App.TLKM.login.ContactMediums[1].Medium.EmailAddress;

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
