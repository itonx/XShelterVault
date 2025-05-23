// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace XShelterVault.Presentation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CredentialsPage : Page
    {
        public CredentialsPage()
        {
            this.InitializeComponent();
            var vm = App.Host.Services.GetService<CredentialsViewModel>();
            this.DataContext = vm;
        }
    }
}
