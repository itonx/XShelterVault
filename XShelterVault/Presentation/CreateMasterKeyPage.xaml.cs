using Microsoft.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace XShelterVault.Presentation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateMasterKeyPage : Page
    {
        public CreateMasterKeyPage()
        {
            this.InitializeComponent();
        }

        private void password_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                var vm = this.DataContext as CreateMasterKeyViewModel;
                vm?.CreateMasterKeyCommand.Execute(null);
            }
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as CreateMasterKeyViewModel;
            vm?.PasswordRequirementsVM?.PasswordChangedCommand.Execute((sender as PasswordBox).Password);
        }
    }
}
