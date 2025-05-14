using Microsoft.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace XShelterVault.Presentation;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ConfirmMasterKeyPage : Page
{
    public ConfirmMasterKeyPage()
    {
        this.InitializeComponent();
    }

    private void password_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            var vm = this.DataContext as ConfirmMasterKeyViewModel;
            vm?.ConfirmMasterKeyCommand.Execute(password.Password);
        }
    }
}
