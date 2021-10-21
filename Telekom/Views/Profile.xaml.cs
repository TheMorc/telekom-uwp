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
            string tempSimLabel = simLabel.Text;
            string tempFirstName = firstName.Text;
            string tempLastName = lastName.Text;
            string tempContactTelephoneNumber = contactTelephoneNumber.Text;
            string tempEmailAddress = emailAddress.Text;

            bool patch_success = await System.Threading.Tasks.Task.Run(() => App.TLKM.PatchProfile(tempSimLabel, tempFirstName, tempLastName, tempContactTelephoneNumber, tempEmailAddress));
            if (patch_success)
            {
                App.TLKM.lastError = App.resourceLoader.GetString("Profile/Success");
                await App.TLKM.ShowMessage();
                App.TLKM.login.ContactMediums[1].Medium.EmailAddress = tempEmailAddress;
                App.TLKM.prodRep.Label = tempSimLabel;
                App.TLKM.login.Individual.GivenName = tempFirstName;
                App.TLKM.login.Individual.FamilyName = tempLastName;
                App.TLKM.serviceId = long.Parse(tempContactTelephoneNumber.Remove(0, 1));

            }
            else
            {
                await App.TLKM.ShowMessage();
            }
        }
    }
}
